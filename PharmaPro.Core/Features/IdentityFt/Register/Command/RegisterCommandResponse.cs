namespace PharmaPro.Core.Features.IdentityFt.Register.Command
{
    public class RegisterCommandResponse
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}