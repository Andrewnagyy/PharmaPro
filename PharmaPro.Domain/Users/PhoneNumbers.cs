using PharmaPro.Domain.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaPro.Domain.Users
{
    public class PhoneNumbers
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}