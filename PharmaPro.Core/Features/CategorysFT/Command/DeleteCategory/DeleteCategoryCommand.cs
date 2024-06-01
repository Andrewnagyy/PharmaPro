using MediatR;
using PharmaPro.Core.Contract.Api;

namespace PharmaPro.Core.Features.CategorysFT.Command.DeleteCategory
{
    public class DeleteCategoryCommand : IRequest<APIResponse<DeleteCategoryCommandResponse>>
    {
        public Guid Id { get; set; }
    }
}
