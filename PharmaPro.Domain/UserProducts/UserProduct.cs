using PharmaPro.Domain.Products;
using PharmaPro.Domain.Users;

namespace PharmaPro.Domain.UserProducts
{
    public class UserProduct
    {

        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
