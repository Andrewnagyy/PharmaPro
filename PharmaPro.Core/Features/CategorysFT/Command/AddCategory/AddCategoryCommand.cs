using MediatR;
using PharmaPro.Core.Contract.Api;

namespace PharmaPro.Core.Features.CategorysFT.Command.AddCategory
{
    public class AddCategoryCommand : IRequest<APIResponse<AddCategoryCommandResponse>>
    {
        public string? Name { get; set; }

    }
}
