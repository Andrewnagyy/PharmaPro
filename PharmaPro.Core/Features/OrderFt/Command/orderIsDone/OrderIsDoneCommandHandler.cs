using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmaPro.Core.Contract.Api;
using PharmaPro.DS;
using System.Net;

namespace PharmaPro.Core.Features.OrderFt.Command.orderIsDone
{
    public class OrderIsDoneCommandHandler : IRequestHandler<OrderIsDoneCommand, APIResponse<OrderIsDoneCommandResponse>>
    {
        private readonly AppDbContext _dbContext;

        public OrderIsDoneCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<APIResponse<OrderIsDoneCommandResponse>> Handle(OrderIsDoneCommand request, CancellationToken cancellationToken)
        {
            var order = await _dbContext.orders
                .Include(o => o.OrderProducts)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId);

            if (order == null)
            {
                return new APIResponse<OrderIsDoneCommandResponse>
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "No order found with this ID."
                };
            }

            order.OrderIsDone = true;

            foreach (var orderProduct in order.OrderProducts)
            {
                if (orderProduct.Product.Amount >= orderProduct.Amount)
                {
                    orderProduct.Product.Amount -= orderProduct.Amount;
                }
                else
                {
                    return new APIResponse<OrderIsDoneCommandResponse>
                    {
                        HttpStatusCode = HttpStatusCode.OK,
                        Message = $"Insufficient amount for product: {orderProduct.Product.Name}"
                    };
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new APIResponse<OrderIsDoneCommandResponse>
            {
                HttpStatusCode = HttpStatusCode.OK,
                Message = "Order status updated successfully."
            };
        }
    }
}