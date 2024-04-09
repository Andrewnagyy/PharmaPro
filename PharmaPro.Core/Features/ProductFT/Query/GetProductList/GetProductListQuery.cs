using MediatR;
using PharmaPro.Core.Contract.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaPro.Core.Features.ProductFT.Query.GetProductList
{
    public record GetProductListQuery : IRequest<APIResponse<GetProductListQueryResponse>>;
}
