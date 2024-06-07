using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Core.Features.IdentityFt.Login.Command;
using PharmaPro.Core.Features.IdentityFt.Register.Command;
using static PharmaPro.Repositories.AuthorizationRepo.AuthorizationRepository;

namespace PharmaPro.Repositories.AuthorizationRepo
{
    public interface IAuthorizationRepository
    {
        Task<APIResponse<RegisterCommandResponse>> RegisterAdminAsync(RegisterCommand command);
        Task<APIResponse<RegisterCommandResponse>> RegisterPharmacistAsync(RegisterCommand command);
        Task<APIResponse<RegisterCommandResponse>> RegisterUserAsync(RegisterCommand command);
        Task<APIResponse<LoginCommandResponse>> LoginAsync(LoginCommand command);
        IQueryable<IdentityUser> GetUserRepo();
        Task<APIResponse<PasswordResetResponse>> RequestPasswordReset(string email);
        Task<APIResponse<IActionResult>> ResetPassword(string token, string email, string newPassword, string confirmPassword);

        Task<APIResponse<string>> DeleteUserAccountAsync(Guid id);

    }
}