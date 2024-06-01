using MediatR;
using PharmaPro.Core.Contract.Api;


namespace PharmaPro.Core.Features.IdentityFt.Register.Command
{
    public record RegisterCommand : IRequest<APIResponse<RegisterCommandResponse>>
    {
        public required string Username { get; init; }
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}