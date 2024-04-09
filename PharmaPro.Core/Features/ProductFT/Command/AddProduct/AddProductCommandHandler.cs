using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Domain.Products;
using PharmaPro.DS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PharmaPro.Core.Features.ProductFT.Command.AddProduct
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, APIResponse<AddProductCommandResponse>>
    {
        private readonly AppDbContext _dbContext;
        public AddProductCommandHandler(AppDbContext appDbContext) => _dbContext = appDbContext;

        public async Task<APIResponse<AddProductCommandResponse>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            bool productExists = await _dbContext.products.AnyAsync(c => c.BarCode == request.BarCode, cancellationToken);
            if (productExists)
            {
                return new APIResponse<AddProductCommandResponse>
                {
                    Errors = new List<string>()
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
                    Errors = new List<string>() { "CategoryId not found" },
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Photo = new List<ProductPhoto>(),
                Amount = request.Amount,
                BarCode = request.BarCode,
                Active = request.Active,
                SoldOut = request.SoldOut,
                Price = request.Price,
                CategoryId = request.CategoryId,
            };

            foreach (var photoId in request.Photos)
            {
                var photo = new ProductPhoto
                {
                    PhotoId = photoId
                };

                product.Photo.Add(photo);
            }

            _dbContext.products.Add(product);
            await _dbContext.SaveChangesAsync(cancellationToken);


            return new APIResponse<AddProductCommandResponse>
            {
                Data = new AddProductCommandResponse()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Photos = product.Photo.Select(p => p.PhotoId).ToList(),
                    Amount = product.Amount,
                    BarCode = product.BarCode,
                    Active = product.Active,
                    SoldOut = product.SoldOut,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    Message = "Product added Successfully"
                },
                HttpStatusCode = HttpStatusCode.Created
            };
        }
    }
}
