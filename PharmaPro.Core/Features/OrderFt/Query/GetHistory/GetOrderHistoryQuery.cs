using MediatR;
using PharmaPro.Core.Contract.Api;

namespace PharmaPro.Core.Features.OrderFt.Query.GetHistory
{
    public class GetOrderHistoryQuery : IRequest<APIResponse<GetOrderHistoryQueryResponse>>
    {
        public Guid userId { get; set; }
    }
}
