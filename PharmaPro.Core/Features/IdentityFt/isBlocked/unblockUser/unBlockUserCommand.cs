using MediatR;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Core.Features.IdentityFt.isBlocked.unblockUser;

namespace PharmaPro.Core.Features.IdentityFt.isBlocked.blockUser
{
    public class unBlockUserCommand : IRequest<APIResponse<unBlockUserCommandResponse>>
    {
        public Guid UserId { get; set; }
    }
}