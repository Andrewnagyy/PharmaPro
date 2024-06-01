using MediatR;
using Microsoft.AspNetCore.Identity;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Core.Features.IdentityFt.isBlocked.blockUser;
using System.Net;

namespace PharmaPro.Core.Features.IdentityFt.isBlocked.unblockUser
{
    public class unBlockUserCommandHandler : IRequestHandler<unBlockUserCommand, APIResponse<unBlockUserCommandResponse>>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public unBlockUserCommandHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<APIResponse<unBlockUserCommandResponse>> Handle(unBlockUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user == null)
            {
                return new APIResponse<unBlockUserCommandResponse>
                {
                    Errors = new List<string> { "User not found" },
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            user.LockoutEnd = null;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new APIResponse<unBlockUserCommandResponse>
                {
                    Data = new unBlockUserCommandResponse()
                    {
                        Message = "Unblocked Successfully"
                    },
                    HttpStatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<unBlockUserCommandResponse>
                {
                    Errors = result.Errors.Select(err => err.Description).ToList(),
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }
        }
    }
}