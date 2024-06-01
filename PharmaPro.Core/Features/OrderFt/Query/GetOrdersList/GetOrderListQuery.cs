using MediatR;
using PharmaPro.Core.Contract.Api;

namespace PharmaPro.Core.Features.OrderFt.Query.GetOrdersList
{
    public class GetOrderListQuery : IRequest<APIResponse<List<OrderDto>>>
    {
    }

    public class OrderDto
    {
        public Guid OrderId { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }
        public bool orderIsDone { get; set; }
        public List<OrderProductDto> OrderProducts { get; set; }
    }

    public class OrderProductDto
    {
        public Guid ProductId { get; set; }
        public string productName { get; set; }
        public decimal productPrice { get; set; }
        public int Amount { get; set; }
    }
}
