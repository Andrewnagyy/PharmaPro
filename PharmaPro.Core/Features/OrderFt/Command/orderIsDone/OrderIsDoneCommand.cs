using MediatR;
using PharmaPro.Core.Contract.Api;

namespace PharmaPro.Core.Features.OrderFt.Command.orderIsDone
{
    public class OrderIsDoneCommand : IRequest<APIResponse<OrderIsDoneCommandResponse>>
    {
        public Guid OrderId { get; set; }

    }
}
