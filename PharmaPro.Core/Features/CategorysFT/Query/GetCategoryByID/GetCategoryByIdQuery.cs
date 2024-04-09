using MediatR;
using PharmaPro.Core.Contract.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaPro.Core.Features.CategorysFT.Query.GetCategoryByID
{
    public class GetproductByIdQuery : IRequest<APIResponse<GetCategoryByIdQueryResponse>>
    {
        public Guid CategoryId { get; set; }
    }
}
