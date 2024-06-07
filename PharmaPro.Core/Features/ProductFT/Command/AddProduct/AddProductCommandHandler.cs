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

namespace PharmaPro.Core.Features.ProductFT.Command.AddProduct
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, APIResponse<AddProductCommandResponse>>
    {
        private readonly string _storagePath;
        private readonly AppDbContext _dbContext;

        public AddProductCommandHandler(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
            _storagePath = Path.Combine(Globals.StorageRootPath, Globals.UploadPath);
        }

        public async Task<APIResponse<AddProductCommandResponse>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            bool productExists = await _dbContext.products.AnyAsync(c => c.BarCode == request.BarCode, cancellationToken);
            if (productExists)
            {
                return new APIResponse<AddProductCommandResponse>
                {
                    Errors = new List<string>
                    {
                        "This Product with this Bar Code already exists"
                    },
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            var category = await _dbContext.categories.FindAsync(request.CategoryId);
            if (category == null)
            {
                return new APIResponse<AddProductCommandResponse>
                {
                    Errors = new List<string> { "CategoryId not found" },
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            // Handle image upload
            Guid photoId = Guid.Empty;
            if (request.PhotoFile != null)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                if (!request.PhotoFile.ContentType.StartsWith("image/"))
                {
                    return new APIResponse<AddProductCommandResponse>
                    {
                        Errors = new List<string>
                        {
                            $"Content Type '{request.PhotoFile.ContentType}' Not Supported here, Only Images are allowed"
                        },
                        HttpStatusCode = HttpStatusCode.BadRequest
                    };
                }

                if (!Directory.Exists(_storagePath))
                    Directory.CreateDirectory(_storagePath);

                string[] fileSplit = request.PhotoFile.FileName.Split('.');
                if (!AllowedExtensions.Get().Contains(fileSplit.Last().ToUpper()))
                {
                    return new APIResponse<AddProductCommandResponse>
                    {
                        Errors = new List<string>
                        {
                            $"The {fileSplit.Last()} is not allowed in this website!"
                        },
                        HttpStatusCode = HttpStatusCode.BadRequest,
                    };
                }

                string fileStoredName = $"{fileSplit.FirstOrDefault()}_{Guid.NewGuid()}.{fileSplit.LastOrDefault()}";
                string fullPath = Path.Combine(_storagePath, fileStoredName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await request.PhotoFile.CopyToAsync(stream);
                }

                //photoPath = fileStoredName;
                var doc = new ImageStorage
                {
                    Id = Guid.NewGuid(),
                    ImageReference = fileStoredName
                };

                await _dbContext.ImagesStorage.AddAsync(doc);
                await _dbContext.SaveChangesAsync(cancellationToken);
                stopwatch.Stop();

                photoId = doc.Id;

            }

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Amount = request.Amount,
                BarCode = request.BarCode,
                Active = request.Active,
                SoldOut = request.SoldOut,
                ExpirationDate = request.ExpirationDate,
                Price = request.Price,
                CategoryId = request.CategoryId,
                Photo = photoId.ToString(),
            };

            _dbContext.products.Add(product);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new APIResponse<AddProductCommandResponse>
            {
                Data = new AddProductCommandResponse
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Photos = product.Photo,
                    Amount = product.Amount,
                    BarCode = product.BarCode,
                    Active = product.Active,
                    SoldOut = product.SoldOut,
                    ExpirationDate = product.ExpirationDate,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    Message = "Product added Successfully"
                },
                HttpStatusCode = HttpStatusCode.Created
            };
        }
    }
}
