using MediatR;
using PharmaPro.Core.Contract.Api;

namespace PharmaPro.Core.Features.CategorysFT.Query.GetCategoryByID
{
    public class GetproductByIdQuery : IRequest<APIResponse<GetCategoryByIdQueryResponse>>
    {
        public Guid CategoryId { get; set; }
    }
}
