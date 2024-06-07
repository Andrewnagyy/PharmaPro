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

namespace PharmaPro.Core.Features.ProductFT.Query.GetProductByCategory
{
    public class GetProductByCategoryHandler : IRequestHandler<GetProductByCategoryQuery, APIResponse<GetProductByCategoryResponse>>
    {
        private readonly AppDbContext _dbContext;
        public GetProductByCategoryHandler(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
            
        }
        public async Task<APIResponse<GetProductByCategoryResponse>> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
        {
            var products = await _dbContext.products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == request.CategoryId)
                .Select(p => new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Photo = p.Photo,
                    Amount = p.Amount,
                    BarCode = p.BarCode,
                    Active = p.Active,
                    SoldOut = p.SoldOut,
                    ExpirationDate = p.ExpirationDate,
                    Price = p.Price,
                    Offer = p.Offer,
                    Discount = p.Discount,
                    OldPrice = p.OldPrice,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name
                })
                .ToListAsync(cancellationToken);

            return new APIResponse<GetProductByCategoryResponse>
            {
                Data = new GetProductByCategoryResponse
                {
                    Products = products
                },
                HttpStatusCode = HttpStatusCode.OK
            };
        }
    }
}
