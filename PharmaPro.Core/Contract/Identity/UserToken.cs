using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PharmaPro.Domain.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace PharmaPro.Core.Contract.Identity
{
    public class UserToken : IUserToken
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;

        public UserToken(IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<TokenPayload> GetTokenPayloadFromToken()
        {
            string jwt = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "") ?? String.Empty;
            if (jwt == null || jwt.Equals(""))
            {
                return new TokenPayload()
                {
                    UserID = Guid.Empty,
                    ExpiredTime = 0
                };
            }
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            String userId = token.Claims.FirstOrDefault(row => row.Type.Equals("Sub"))?.Value ?? "";
            Guid userID = Guid.Parse(userId);
            long expiredTime = long.Parse(token.Claims.FirstOrDefault(row => row.Type.Equals("exp"))?.Value ?? String.Empty);
            String roleStr = (token.Claims.FirstOrDefault(row => row.Type.Equals("role"))?.Value ?? String.Empty);

            RoleEnum? Role = ConvertToEnum(roleStr);

            return await Task.FromResult(new TokenPayload()
            {
                UserID = userID,
                ExpiredTime = expiredTime,
                Role = Role
            });
        }

        public async Task<Guid> GetUserIDFromToken()
        {
            string jwt = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "") ?? String.Empty;
            if (jwt == null || jwt.Equals(""))
            {
                return Guid.Empty;
            }
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwt);
                String userId = token.Claims.FirstOrDefault(row => row.Type.Equals("UserID"))?.Value ?? "";
                Guid userID = Guid.Parse(userId);
                return await Task.FromResult(userID);
            }
            catch (Exception)
            {
                return Guid.Empty;
            }
        }

        public Guid GetUserIDFromTokenNotAsync()
        {
            string jwt = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "") ?? String.Empty;
            if (jwt == null || jwt.Equals(""))
            {
                return Guid.Empty;
            }
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            String userId = token.Claims.FirstOrDefault(row => row.Type.Equals("Sub"))?.Value ?? "";
            Guid userID = Guid.Parse(userId);
            return userID;
        }

        public async Task<string> GetUserEmailFromToken()
        {
            string jwt = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "") ?? string.Empty;

            if (string.IsNullOrEmpty(jwt))
            {
                return string.Empty;
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwt);

                // Attempt to retrieve email claim from token
                string email = token.Claims.FirstOrDefault(claim => claim.Type == "email")?.Value;

                // If email claim is missing or null, handle it appropriately
                if (string.IsNullOrEmpty(email))
                {
                    // Handle the case where email claim is missing
                    // You can return a default value or throw an exception depending on your requirements
                    // For example:
                    // throw new Exception("Email claim not found in JWT token.");
                    return string.Empty; // Return an empty string as default email
                }

                return await Task.FromResult(email);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error while extracting email from token: {ex.Message}");
                return string.Empty; // Return an empty string in case of any exception
            }
        }

        public async Task<string> GetUserEmailById(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            return user?.Email;
        }


        private RoleEnum? ConvertToEnum(string str)
        {
            foreach (RoleEnum value in Enum.GetValues(typeof(RoleEnum)))
            {
                if (string.Equals(value.ToString(), str, StringComparison.OrdinalIgnoreCase))
                {
                    return value;
                }
            }
            return null;
        }
    }
}
