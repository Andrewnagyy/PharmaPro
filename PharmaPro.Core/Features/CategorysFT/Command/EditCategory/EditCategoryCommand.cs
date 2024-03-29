using MediatR;
using PharmaPro.Core.Contract.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaPro.Core.Features.CategorysFT.Command.EditCategory
{
    public class EditCategoryCommand : IRequest<APIResponse<EditCategoryCommandResponse>>
    {
        public Guid CategoryId { get; set; }
        public string? NewName { get; set; }
    }
}
