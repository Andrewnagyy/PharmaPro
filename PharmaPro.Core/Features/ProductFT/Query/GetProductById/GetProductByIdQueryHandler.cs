using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Core.Features.CategorysFT.Query.GetCategoryByID;
using PharmaPro.Domain.Products;
using PharmaPro.DS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PharmaPro.Core.Features.ProductFT.Query.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, APIResponse<GetProductByIdQueryResponse>>
    {
        private readonly AppDbContext _dbContext;
        public GetProductByIdQueryHandler(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;

        }
        public async Task<APIResponse<GetProductByIdQueryResponse>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var productExist = await _dbContext.products
                .Include(p => p.Photo)
                .FirstOrDefaultAsync(p => p.Id == query.Id, cancellationToken);

            if (productExist == null)
            {
                return new APIResponse<GetProductByIdQueryResponse>()
                {
                    Errors = new List<string>()
                {
                    "Product is Not Found!"
                },
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound,
                };
            }
            else
            {
                var product = await _dbContext.products.FindAsync(query.Id);

                return new APIResponse<GetProductByIdQueryResponse>()
                {
                    Data = new GetProductByIdQueryResponse()
                    { 
                        Name = product.Name,
                        Description = product.Description,
                        Photo = product.Photo.Select(p => p.PhotoId).ToList(),
                        Amount = product.Amount,
                        BarCode = product.BarCode,
                        Active = product.Active,
                        SoldOut = product.SoldOut,
                        Price = product.Price,
                        CategoryId = product.CategoryId,
                    },
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                };
            }
        }
    }
}
