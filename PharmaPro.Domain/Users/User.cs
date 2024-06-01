using PharmaPro.Domain.UserProducts;

namespace PharmaPro.Domain.Users
{
    public class User
    {
        public Guid UserID { get; set; }
        public string Name { get; set; }
        public string Gmail { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Age { get; set; }
        public string PhoneNumber { get; set; }
        public string ChronicDisease { get; set; }

        public ICollection<UserProduct> UserProducts { get; set; }

    }
}
