using MediatR;
using PharmaPro.Core.Contract.Api;

namespace PharmaPro.Core.Features.ProductFT.Command.DeleteProduct
{
    public class DeleteProductCommand : IRequest<APIResponse<DeleteProductCommandResponse>>
    {
        public Guid Id { get; set; }
    }
}
