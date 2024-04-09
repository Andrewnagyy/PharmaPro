using PharmaPro.Domain.Identity;

namespace PharmaPro.Core.Contract.Identity
{
    public struct TokenPayload
    {
        public Guid UserID { get; set; }
        public long ExpiredTime { get; set; }
        public RoleEnum? Role { get; set; }
    }
}
