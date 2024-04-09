using MediatR;
using PharmaPro.Core.Contract.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaPro.Core.Features.ProductFT.Command.DeleteProduct
{
    public class DeleteProductCommand : IRequest<APIResponse<DeleteProductCommandResponse>>
    {
        public Guid Id { get; set; }
    }
}
