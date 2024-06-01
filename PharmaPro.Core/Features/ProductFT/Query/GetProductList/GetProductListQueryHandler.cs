using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmaPro.Core.Contract.Api;
using PharmaPro.DS;
using System.Net;

namespace PharmaPro.Core.Features.ProductFT.Query.GetProductList
{
    public record ProductDto(Guid Id, string Name, string Description, string Photo, int Amount, string BarCode, bool Active, bool SoldOut, DateTime ExpirationDate, decimal Price, bool Offer, int Discount, decimal OldPrice, Guid CategoryID);

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
                    $"https://pharmapro.somee.com/api/Storage/GetImageById?id={p.Photo}",
                    p.Amount,
                    p.BarCode,
                    p.Active,
                    p.SoldOut,
                    p.ExpirationDate,
                    p.Price,
                    p.Offer,
                    p.Discount,
                    p.OldPrice,
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
