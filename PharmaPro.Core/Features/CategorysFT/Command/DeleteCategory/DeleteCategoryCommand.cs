using MediatR;
using PharmaPro.Core.Contract.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaPro.Core.Features.CategorysFT.Command.DeleteCategory
{ 
      public class DeleteCategoryCommand : IRequest<APIResponse<DeleteCategoryCommandResponse>>
      {
            public Guid CategoryId { get; set; }
      }
}
