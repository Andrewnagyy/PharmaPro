using MediatR;
using PharmaPro.Core.Contract.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaPro.Core.Features.ProductFT.Query.GetProductByCategory
{
    public class GetProductByCategoryQuery : IRequest<APIResponse<GetProductByCategoryResponse>>
    {
        public Guid CategoryId { get; set; }

    }
}
