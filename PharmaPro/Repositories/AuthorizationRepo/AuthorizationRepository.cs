using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PharmaPro.Core.Contract;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Core.Features.IdentityFt.Login.Command;
using PharmaPro.Core.Features.IdentityFt.Register.Command;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using static PharmaPro.SendGrid.Service.EmailSenderService;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PharmaPro.Repositories.AuthorizationRepo
{
    public class AuthorizationRepository : IAuthorizationRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
    private readonly IEmailSender _emailSender;
        private readonly IConfiguration _config;

        private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUrlHelper _urlHelper;

    public AuthorizationRepository(
            UserManager<IdentityUser> userManager,
            IEmailSender emailSender,
            IHttpContextAccessor httpContextAccessor,
            IUrlHelperFactory urlHelperFactory,
            IConfiguration config)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;

            var actionContext = new ActionContext(_httpContextAccessor.HttpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContext);
            _config = config;
        }

        public string GenerateTokenString(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,"Admin"),
                new Claim("UserID", user.Id)
            };

            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));

            var signingCred = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMonths(30),
                issuer: _config.GetSection("Jwt:Issuer").Value,
                audience: _config.GetSection("Jwt:Audience").Value,
                signingCredentials: signingCred);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public IQueryable<IdentityUser> GetUserRepo()
        {
            return _userManager.Users;
        }

        public async Task<APIResponse<LoginCommandResponse>> LoginAsync(LoginCommand command)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);

            if (user == null)
            {
                return new APIResponse<LoginCommandResponse>
                {
                    Errors = new List<string> { "Invalid email or password." },
                    HttpStatusCode = HttpStatusCode.Unauthorized
                };
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                return new APIResponse<LoginCommandResponse>
                {
                    Errors = new List<string> { "Your account is locked due to multiple failed login attempts. Please contact support." },
                    Message = "Account locked due to multiple failed login attempts. Please contact support.",
                    HttpStatusCode = HttpStatusCode.Locked
                };
            }

            if (await _userManager.CheckPasswordAsync(user, command.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                string token = GenerateTokenString(user);
                var response = new APIResponse<LoginCommandResponse>
                {
                    Data = new LoginCommandResponse()
                    {
                        Token = token,
                        Role = roles.FirstOrDefault(),
                        username = user.UserName,
                        ID = user.Id
                    },
                    HttpStatusCode = HttpStatusCode.OK
                };
                await _userManager.ResetAccessFailedCountAsync(user);

                return response;
            }
            else
            {
                await _userManager.AccessFailedAsync(user);

                if (await _userManager.IsLockedOutAsync(user))
                {
                    return new APIResponse<LoginCommandResponse>
                    {
                        Errors = new List<string> { "Your account is locked due to multiple failed login attempts. Please contact support." },
                        HttpStatusCode = HttpStatusCode.Locked, // Use 423 Locked status code
                        Message = "Account locked due to multiple failed login attempts. Please contact support."
                    };
                }

                return new APIResponse<LoginCommandResponse>
                {
                    Errors = new List<string> { "Invalid email or password." },
                    HttpStatusCode = HttpStatusCode.Unauthorized
                };
            }
        }


        public async Task<APIResponse<RegisterCommandResponse>> RegisterUserAsync(RegisterCommand command)
        {
            bool UserIsExist = await _userManager.Users.AnyAsync(GetUserExistFilter(command.Email));
            if (UserIsExist)
            {
                return new APIResponse<RegisterCommandResponse>
                {
                    Errors = new List<string>()
                        {
                            "That email address is already in use"
                        },
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            var newUser = new IdentityUser
            {
                UserName = command.Username,
                Email = command.Email,

            };

            var result = await _userManager.CreateAsync(newUser, command.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "User");
                string token = GenerateTokenString(newUser);
                var roles = await _userManager.GetRolesAsync(newUser);

                return new APIResponse<RegisterCommandResponse>
                {
                    Data = new RegisterCommandResponse()
                    {
                        userId = newUser.Id,
                        Email = command.Email,
                        Token = token,
                        Role = roles.FirstOrDefault()
                    },
                    HttpStatusCode = HttpStatusCode.Created
                };
            }
            return new APIResponse<RegisterCommandResponse>
            {
                Errors = result.Errors.Select(err => err.Description).ToList(),
                HttpStatusCode = HttpStatusCode.BadRequest
            };
        }

        public async Task<APIResponse<RegisterCommandResponse>> RegisterAdminAsync(RegisterCommand command)
        {
            bool UserIsExist = await _userManager.Users.AnyAsync(GetUserExistFilter(command.Email));
            if (UserIsExist)
            {
                return new APIResponse<RegisterCommandResponse>
                {
                    Errors = new List<string>()
                    {
                        "That email address is already in use"
                    },
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            var newUser = new IdentityUser
            {
                UserName = command.Username,
                Email = command.Email,
            };

            var result = await _userManager.CreateAsync(newUser, command.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "Admin");
                string token = GenerateTokenString(newUser);
                var roles = await _userManager.GetRolesAsync(newUser);

                return new APIResponse<RegisterCommandResponse>
                {
                    Data = new RegisterCommandResponse()
                    {
                        Email = command.Email,
                        Token = token,
                        Role = roles.FirstOrDefault()

                    },
                    HttpStatusCode = HttpStatusCode.Created
                };
            }
            return new APIResponse<RegisterCommandResponse>
            {
                Errors = result.Errors.Select(err => err.Description).ToList(),
                HttpStatusCode = HttpStatusCode.BadRequest
            };
        }


        public async Task<APIResponse<RegisterCommandResponse>> RegisterPharmacistAsync(RegisterCommand command)
        {
            bool UserIsExist = await _userManager.Users.AnyAsync(GetUserExistFilter(command.Email));
            if (UserIsExist)
            {
                return new APIResponse<RegisterCommandResponse>
                {
                    Errors = new List<string>()
                    {
                        "That email address is already in use"
                    },
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            var newUser = new IdentityUser
            {
                UserName = command.Username,
                Email = command.Email,
            };

            var result = await _userManager.CreateAsync(newUser, command.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "Pharmacist");
                string token = GenerateTokenString(newUser);
                var roles = await _userManager.GetRolesAsync(newUser);

                return new APIResponse<RegisterCommandResponse>
                {
                    Data = new RegisterCommandResponse()
                    {
                        Email = command.Email,
                        Token = token,
                        Role = roles.FirstOrDefault()

                    },
                    HttpStatusCode = HttpStatusCode.Created
                };
            }
            return new APIResponse<RegisterCommandResponse>
            {
                Errors = result.Errors.Select(err => err.Description).ToList(),
                HttpStatusCode = HttpStatusCode.BadRequest
            };
        }

        public async Task<APIResponse<string>> BlockUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new APIResponse<string>
                {
                    Errors = new List<string> { "User not found" },
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return new APIResponse<string>
                {
                    Errors = new List<string> { "Cannot block an admin user" },
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            user.LockoutEnd = DateTimeOffset.MaxValue;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new APIResponse<string>
                {
                    Data = "User blocked successfully",
                    HttpStatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new APIResponse<string>
                {
                    Errors = result.Errors.Select(err => err.Description).ToList(),
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public async Task<APIResponse<PasswordResetResponse>> RequestPasswordReset(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new APIResponse<PasswordResetResponse>
                {
                    Errors = new List<string> { "Invalid email address" },
                    HttpStatusCode = HttpStatusCode.OK
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = _urlHelper.Action("ResetPassword", "Account", new { token, email = user.Email }, _httpContextAccessor.HttpContext.Request.Scheme);

            await _emailSender.SendEmailAsync(email, "Password Reset Request",
                $"Please reset your password by clicking here: <a href=\"{resetLink}\">link</a>");

            return new APIResponse<PasswordResetResponse>
            {
                Data = new PasswordResetResponse
                {
                    Token = token,
                    Message = "Password reset link sent to your email"
                },
                HttpStatusCode = HttpStatusCode.OK
            };
        }


        public async Task<APIResponse<IActionResult>> ResetPassword(string email, string token,string newPassword, string confirmPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new APIResponse<IActionResult>
                {
                    Errors = new List<string> { "Invalid email address" },
                   
                    HttpStatusCode = HttpStatusCode.OK
                };
            }

            if (newPassword != confirmPassword)
            {
                return new APIResponse<IActionResult>
                {
                    Errors = new List<string> { "Passwords do not match" },
                    HttpStatusCode = HttpStatusCode.OK
                };
            }

            var resetResult = await _userManager.ResetPasswordAsync(user,token, newPassword);
            if (!resetResult.Succeeded)
            {
                return new APIResponse<IActionResult>
                {
                    Errors = resetResult.Errors.Select(err => err.Description).ToList(),
                    HttpStatusCode = HttpStatusCode.OK
                };
            }

            return new APIResponse<IActionResult>
            {
                Errors = new List<string> { "Password reset successfully" },
                HttpStatusCode = HttpStatusCode.OK
            };
        }

        private Expression<Func<IdentityUser, bool>> GetUserExistFilter(string email)
        {
            email = email.ToLower();
            return user => (user.Email != null && user.Email.ToLower().Equals(email));
        }
        public class PasswordResetResponse
        {
            public string Token { get; set; }
            public string Message { get; set; }   
        }
    }
}