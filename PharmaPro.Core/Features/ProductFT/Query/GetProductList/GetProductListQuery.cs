using MediatR;
using PharmaPro.Core.Contract.Api;

namespace PharmaPro.Core.Features.ProductFT.Query.GetProductList
{
    public record GetProductListQuery : IRequest<APIResponse<GetProductListQueryResponse>>;
}
