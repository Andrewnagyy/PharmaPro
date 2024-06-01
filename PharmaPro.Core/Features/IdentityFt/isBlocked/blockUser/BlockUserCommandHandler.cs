using MediatR;
using Microsoft.AspNetCore.Identity;
using PharmaPro.Core.Contract.Api;
using System.Net;

namespace PharmaPro.Core.Features.IdentityFt.isBlocked.blockUser
{
    public class BlockUserCommandHandler : IRequestHandler<BlockUserCommand, APIResponse<BlockUserCommandResponse>>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public BlockUserCommandHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<APIResponse<BlockUserCommandResponse>> Handle(BlockUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user == null)
            {
                return new APIResponse<BlockUserCommandResponse>
                {
                    Errors = new List<string> { "User not found" },
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return new APIResponse<BlockUserCommandResponse>
                {
                    Errors = new List<string> { "Cannot block an admin user" },
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            user.LockoutEnd = DateTimeOffset.MaxValue;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new APIResponse<BlockUserCommandResponse>
                {
                    Data = new BlockUserCommandResponse()
                    {
                        Message = "Blocked Successfully"
                    },

                    HttpStatusCode = HttpStatusCode.OK
                };

            }
            else
            {
                return new APIResponse<BlockUserCommandResponse>
                {
                    Errors = result.Errors.Select(err => err.Description).ToList(),
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }
        }
    }

}
