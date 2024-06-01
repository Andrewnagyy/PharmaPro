using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmaPro.Core.Contract.Api;
using PharmaPro.DS;
using System.Net;

namespace PharmaPro.Core.Features.ProductFT.Command.EditProduct
{
    public class EditProductCommandHandler : IRequestHandler<EditProductCommand, APIResponse<EditProductCommandResponse>>
    {
        private readonly AppDbContext _dbContext;
        public EditProductCommandHandler(AppDbContext dbContext) => _dbContext = dbContext;

        public async Task<APIResponse<EditProductCommandResponse>> Handle(EditProductCommand request, CancellationToken cancellationToken)
        {

            var product = await _dbContext.products
          .Include(p => p.Photo)
          .FirstOrDefaultAsync(p => p.Id == request.Id);

            if (product == null)
            {
                return new APIResponse<EditProductCommandResponse>
                {
                    Errors = new List<string> { "Product not found" },
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            // Check if the new barcode exists for another product
            bool barcodeExists = await _dbContext.products.AnyAsync(p => p.BarCode == request.BarCode && p.Id != request.Id, cancellationToken);
            if (barcodeExists)
            {
                return new APIResponse<EditProductCommandResponse>
                {
                    Errors = new List<string> { "Another product with this barcode already exists" },
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            // Check if the category exists
            var categoryExists = await _dbContext.categories.FindAsync(request.CategoryId);
            if (categoryExists == null)
            {
                return new APIResponse<EditProductCommandResponse>
                {
                    Errors = new List<string> { "Category not found" },
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Photo = request.Photo;
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
                    Message = "Product updated SuccessFully"
                },
                HttpStatusCode = HttpStatusCode.OK
            };
        }
    }
}
