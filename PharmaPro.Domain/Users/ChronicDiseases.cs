using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaPro.Domain.Users
{
    public class ChronicDiseases
    {
        public Guid Id { get; set; }
        public string ChronicDisease { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}