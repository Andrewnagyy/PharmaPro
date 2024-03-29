using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Core.Features.IdentityFt.Login.Command;
using PharmaPro.Core.Features.IdentityFt.Register.Command;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace PharmaPro.Repositories.AuthorizationRepo
{
    public class AuthorizationRepository : IAuthorizationRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;
        public AuthorizationRepository(UserManager<IdentityUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
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
            
                if (user != null && await _userManager.CheckPasswordAsync(user, command.Password))
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    String Token = GenerateTokenString(user);
                    APIResponse<LoginCommandResponse> response = new APIResponse<LoginCommandResponse>
                    {
                        Data = new LoginCommandResponse()
                        {
                            Token = Token,
                            Role = roles.FirstOrDefault()
                        },
                        HttpStatusCode = HttpStatusCode.OK
                    };
                    return response;
                }
                else
                {
                    int AccessFailedCount = 0;
                    if (user != null)
                    {
                        await _userManager.AccessFailedAsync(user);
                        AccessFailedCount = await _userManager.GetAccessFailedCountAsync(user);
                    }
                    return new APIResponse<LoginCommandResponse>
                    {
                        Errors = new List<string>()
                        {
                            "Invalid Login!",
                            $"Access Failed Count : {AccessFailedCount}"
                        },
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

        private Expression<Func<IdentityUser, bool>> GetUserExistFilter(String email)
        {
            email = email.ToLower();
            return user => (user.Email != null && user.Email.ToLower().Equals(email));
        }
    }
}