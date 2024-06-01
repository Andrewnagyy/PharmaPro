using MediatR;
using PharmaPro.Core.Contract.Api;

namespace PharmaPro.Core.Features.IdentityFt.isBlocked.blockUser
{
    public class BlockUserCommand : IRequest<APIResponse<BlockUserCommandResponse>>
    {
        public Guid UserId { get; set; }
    }

}
