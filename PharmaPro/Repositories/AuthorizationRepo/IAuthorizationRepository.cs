using Microsoft.AspNetCore.Identity;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Core.Features.IdentityFt.Login.Command;
using PharmaPro.Core.Features.IdentityFt.Register.Command;

namespace PharmaPro.Repositories.AuthorizationRepo
{
    public interface IAuthorizationRepository
    {
        Task<APIResponse<RegisterCommandResponse>> RegisterAdminAsync(RegisterCommand command);
        Task<APIResponse<RegisterCommandResponse>> RegisterPharmacistAsync(RegisterCommand command);
        Task<APIResponse<RegisterCommandResponse>> RegisterUserAsync(RegisterCommand command);
        Task<APIResponse<LoginCommandResponse>> LoginAsync(LoginCommand command);
        IQueryable<IdentityUser> GetUserRepo();
    }
}