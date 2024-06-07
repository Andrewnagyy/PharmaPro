using MediatR;
using PharmaPro.Core.Contract.Api;

namespace PharmaPro.Core.Features.CategorysFT.Query.GetCategoryList
{
    public record GetCategoryListQuery : IRequest<APIResponse<GetCategoryListQueryResponse>>;
}
