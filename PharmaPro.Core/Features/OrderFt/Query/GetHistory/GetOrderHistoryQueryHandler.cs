using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmaPro.Core.Contract.Api;
using PharmaPro.DS;
using System.Net;

namespace PharmaPro.Core.Features.OrderFt.Query.GetHistory
{
    public class GetOrderHistoryQueryHandler : IRequestHandler<GetOrderHistoryQuery, APIResponse<GetOrderHistoryQueryResponse>>
    {
        private readonly AppDbContext _dbContext;

        public GetOrderHistoryQueryHandler(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task<APIResponse<GetOrderHistoryQueryResponse>> Handle(GetOrderHistoryQuery request, CancellationToken cancellationToken)
        {
            var orderHistory = await _dbContext.orders
                .Where(o => o.UserId == request.userId)
                .Select(o => new GetOrderHistoryQueryResponse.OrderHistoryItem
                {
                    OrderId = o.Id,
                    CreatedAt = o.CreatedAt,
                    TotalPrice = o.TotalPrice,
                    OrderIsDone = o.OrderIsDone,
                    orderProducts = o.OrderProducts.Select(oi => new GetOrderHistoryQueryResponse.OrderProduct
                    {
                        ProductId = oi.Product.Id,
                        Amount = oi.Amount,
                        productName = _dbContext.products
                    .Where(p => p.Id == oi.ProductId)
                    .Select(p => p.Name)
                    .FirstOrDefault(),
                        productPrice = _dbContext.products
                    .Where(p => p.Id == oi.ProductId)
                    .Select(p => p.Price)
                    .FirstOrDefault(),
                    }).ToList()
                })
                .ToListAsync(cancellationToken);

            if (orderHistory.Count == 0)
            {
                var response = new APIResponse<GetOrderHistoryQueryResponse>
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "This user has no orders."
                };

                return response;
            }

            var successResponse = new APIResponse<GetOrderHistoryQueryResponse>
            {
                Data = new GetOrderHistoryQueryResponse
                {
                    OrderHistory = orderHistory
                },
                HttpStatusCode = HttpStatusCode.OK
            };

            return successResponse;
        }
    }
}