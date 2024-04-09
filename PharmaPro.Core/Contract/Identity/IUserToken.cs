namespace PharmaPro.Core.Contract.Identity
{
    public interface IUserToken
    {
        Task<Guid> GetUserIDFromToken();
        Guid GetUserIDFromTokenNotAsync();
        Task<TokenPayload> GetTokenPayloadFromToken();
        Task<string> GetUserEmailFromToken();
    }
}
