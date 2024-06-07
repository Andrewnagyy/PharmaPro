using MediatR;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Core.Contract.Identity;
using PharmaPro.DS;
using System.Net;

namespace PharmaPro.Core.Features.OrderFt.Query.GetOrdersList
{
    public class GetOrderListQueryHandler : IRequestHandler<GetOrderListQuery, APIResponse<List<OrderDto>>>
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserToken _userToken;

        public GetOrderListQueryHandler(AppDbContext appDbContext, IUserToken userToken)
        {
            _dbContext = appDbContext;
            _userToken = userToken;
        }

        public async Task<APIResponse<List<OrderDto>>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
        {
            List<OrderDto> orders = _dbContext.orders
                .Select(o => new OrderDto
                {
                    OrderId = o.Id,
                    UserName = o.UserName,
                    Address = o.Address,
                    Date = o.CreatedAt,
                    TotalPrice = o.TotalPrice,
                    orderIsDone = o.OrderIsDone,
                    OrderProducts = o.OrderProducts.Select(oi => new OrderProductDto
                    {
                        ProductId = oi.ProductId,
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
                .OrderBy(o => o.orderIsDone)
                .ToList();

            if (orders.Count == 0)
            {
                return new APIResponse<List<OrderDto>>
                {
                    Data = null,
                    Message = "No orders found",
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            return new APIResponse<List<OrderDto>>
            {
                Data = orders,
                HttpStatusCode = HttpStatusCode.OK
            };
        }
    }
}