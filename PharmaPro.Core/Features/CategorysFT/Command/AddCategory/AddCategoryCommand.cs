using MediatR;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Core.Features.CategorysFT.Command.AddCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaPro.Core.Features.CategorysFT.Command.AddCategory
{
    public class AddCategoryCommand : IRequest<APIResponse<AddCategoryCommandResponse>>
    {
        public string? Name { get; set; }

    }
}
