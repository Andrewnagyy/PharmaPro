using MediatR;
using PharmaPro.Core.Contract.Api;

namespace PharmaPro.Core.Features.OrderFt.Command.AddOrder
{
    public class AddOrderCommand : IRequest<APIResponse<AddOrderCommandResponse>>
    {
        public string UserName { get; set; }
        public string Address { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<orderProductsDto> orderProducts { get; set; }
    }

    public class orderProductsDto
    {
        public Guid ProductId { get; set; }
        public int Amount { get; set; }
    }
}
