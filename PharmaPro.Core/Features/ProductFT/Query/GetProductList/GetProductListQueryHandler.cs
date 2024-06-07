using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmaPro.Core.Contract.Api;
using PharmaPro.DS;
using System.Linq;
using System.Net;

namespace PharmaPro.Core.Features.ProductFT.Query.GetProductList
{
    public record ProductDto(Guid Id, string Name, string Description, string Photo, int Amount, string BarCode, bool Active, bool SoldOut, DateTime ExpirationDate, decimal Price, bool Offer, int Discount, decimal OldPrice, Guid CategoryID, string categoryName);

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
                .Include(p => p.Category)
                .Where(p => p.Active)
                .ToListAsync(cancellationToken);

            if (products == null || !products.Any())
            {
                return new APIResponse<GetProductListQueryResponse>
                {
                    Errors = new List<string> { "No active products found!" },
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            var productsDto = await _dbContext.products
                .Include(p => p.Category)
                .Where(p => p.Active)
                .Select(p => new ProductDto(
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
                    p.CategoryId,
                    p.Category.Name
                ))
                .ToListAsync(cancellationToken);

            var response = new GetProductListQueryResponse(productsDto);

            return new APIResponse<GetProductListQueryResponse>
            {
                Data = response,
                HttpStatusCode = HttpStatusCode.OK
            };
        }
    }
}
