using MediatR;
using PharmaPro.Core.Contract.Api;

namespace PharmaPro.Core.Features.ProductFT.Query.GetProductById
{
    public class GetProductByIdQuery : IRequest<APIResponse<GetProductByIdQueryResponse>>
    {
        public Guid Id { get; set; }
    }
}
