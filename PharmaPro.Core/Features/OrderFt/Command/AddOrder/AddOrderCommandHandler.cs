using MediatR;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Core.Contract.Identity;
using PharmaPro.Domain.Orders;
using PharmaPro.DS;
using System.Net;

namespace PharmaPro.Core.Features.OrderFt.Command.AddOrder
{
    public class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, APIResponse<AddOrderCommandResponse>>
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserToken _userToken;
        public AddOrderCommandHandler(AppDbContext appDbContext, IUserToken userToken)
        {
            _dbContext = appDbContext;
            _userToken = userToken;
        }
        public async Task<APIResponse<AddOrderCommandResponse>> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            Guid userId = await _userToken.GetUserIDFromToken();

            var order = new Order
            {
                UserId = userId,
                UserName = request.UserName,
                Address = request.Address,
                TotalPrice = request.TotalPrice,
                CreatedAt = DateTime.Now,
                OrderProducts = new List<OrderProducts>()
            };

            foreach (var item in request.orderProducts)
            {
                order.OrderProducts.Add(new OrderProducts
                {
                    ProductId = item.ProductId,
                    Amount = item.Amount
                });
            }

            _dbContext.orders.Add(order);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new APIResponse<AddOrderCommandResponse>
            {
                Data = new AddOrderCommandResponse()
                {
                    Message = "Order added successfully"
                },
                HttpStatusCode = HttpStatusCode.Created
            };
        }
    }
}
