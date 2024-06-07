using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Core.Features.Storageft;
using PharmaPro.Core.Helpers;
using PharmaPro.Domain.Products;
using PharmaPro.Domain.Storage;
using PharmaPro.DS;
using System.Diagnostics;
using System.Net;

namespace PharmaPro.Core.Features.ProductFT.Command.EditProduct
{
    public class EditProductCommandHandler : IRequestHandler<EditProductCommand, APIResponse<EditProductCommandResponse>>
    {
        private readonly string _storagePath;
        private readonly AppDbContext _dbContext;

        public EditProductCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _storagePath = Path.Combine(Globals.StorageRootPath, Globals.UploadPath);
        }

        public async Task<APIResponse<EditProductCommandResponse>> Handle(EditProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _dbContext.products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product == null)
            {
                return new APIResponse<EditProductCommandResponse>
                {
                    Errors = new List<string> { "Product not found" },
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            bool barcodeExists = await _dbContext.products.AnyAsync(p => p.BarCode == request.BarCode && p.Id != request.Id, cancellationToken);
            if (barcodeExists)
            {
                return new APIResponse<EditProductCommandResponse>
                {
                    Errors = new List<string> { "Another product with this barcode already exists" },
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            var categoryExists = await _dbContext.categories.FindAsync(request.CategoryId);
            if (categoryExists == null)
            {
                return new APIResponse<EditProductCommandResponse>
                {
                    Errors = new List<string> { "Category not found" },
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            Guid photoId = Guid.Parse(product.Photo); // Keep existing photo ID by default
            if (request.PhotoFile != null)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                if (!request.PhotoFile.ContentType.StartsWith("image/"))
                {
                    return new APIResponse<EditProductCommandResponse>
                    {
                        Errors = new List<string> { $"Content Type '{request.PhotoFile.ContentType}' Not Supported here, Only Images are allowed" },
                        HttpStatusCode = HttpStatusCode.BadRequest
                    };
                }

                if (!Directory.Exists(_storagePath))
                    Directory.CreateDirectory(_storagePath);

                string[] fileSplit = request.PhotoFile.FileName.Split('.');
                if (!AllowedExtensions.Get().Contains(fileSplit.Last().ToUpper()))
                {
                    return new APIResponse<EditProductCommandResponse>
                    {
                        Errors = new List<string> { $"The {fileSplit.Last()} is not allowed in this website!" },
                        HttpStatusCode = HttpStatusCode.BadRequest
                    };
                }

                string fileStoredName = $"{fileSplit.FirstOrDefault()}_{Guid.NewGuid()}.{fileSplit.LastOrDefault()}";
                string fullPath = Path.Combine(_storagePath, fileStoredName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await request.PhotoFile.CopyToAsync(stream);
                }

                var newPhoto = new ImageStorage
                {
                    Id = Guid.NewGuid(),
                    ImageReference = fileStoredName
                };

                await _dbContext.ImagesStorage.AddAsync(newPhoto);
                await _dbContext.SaveChangesAsync(cancellationToken);
                stopwatch.Stop();

                photoId = newPhoto.Id;
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Photo = photoId.ToString();
            product.Amount = request.Amount;
            product.BarCode = request.BarCode;
            product.Active = request.Active;
            product.SoldOut = request.SoldOut;
            product.ExpirationDate = request.ExpirationDate;
            product.Price = request.Price;
            product.CategoryId = request.CategoryId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new APIResponse<EditProductCommandResponse>
            {
                Data = new EditProductCommandResponse
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Photo = product.Photo,
                    Amount = product.Amount,
                    BarCode = product.BarCode,
                    IsActive = product.Active,
                    IsSoldOut = product.SoldOut,
                    ExpirationDate = product.ExpirationDate,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    Message = "Product updated successfully"
                },
                HttpStatusCode = HttpStatusCode.OK
            };
        }
    }
}
