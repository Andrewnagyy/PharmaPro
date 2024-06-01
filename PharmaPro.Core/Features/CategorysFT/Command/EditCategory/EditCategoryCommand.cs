using MediatR;
using PharmaPro.Core.Contract.Api;

namespace PharmaPro.Core.Features.CategorysFT.Command.EditCategory
{
    public class EditCategoryCommand : IRequest<APIResponse<EditCategoryCommandResponse>>
    {
        public Guid CategoryId { get; set; }
        public string? NewName { get; set; }
    }
}
