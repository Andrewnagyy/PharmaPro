using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Core.Features.CategorysFT.Query.GetCategoryList;
using PharmaPro.DS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PharmaPro.Core.Features.ProductFT.Query.GetProductList
{
    public record ProductDto(Guid Id, string Name, string Description, string Photo, int Amount, string BarCode, bool Active, bool SoldOut, decimal Price, Guid CategoryID);

    public class GetProductListQueryHandler : IRequestHandler<GetProductListQuery, APIResponse<GetProductListQueryResponse>>
    {
        private readonly AppDbContext _dbContext;
        public GetProductListQueryHandler(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task<APIResponse<GetProductListQueryResponse>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
        {
            var products = await _dbContext.products
                .Include(p => p.Photo)
                .ToListAsync(cancellationToken);

            if (products == null || !products.Any())
            {
                return new APIResponse<GetProductListQueryResponse>
                {
                    Errors = new List<string> { "No Products found!" },
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            var response = new GetProductListQueryResponse(
                products.Select(p => new ProductDto(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Photo.FirstOrDefault()?.PhotoId,
                    p.Amount,
                    p.BarCode,
                    p.Active,
                    p.SoldOut,
                    p.Price,
                    p.CategoryId
                )).ToList()
            );

            return new APIResponse<GetProductListQueryResponse>
            {
                Data = response,
                HttpStatusCode = HttpStatusCode.OK
            };
        }
    }
}
