using PharmaPro.Domain.Products;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaPro.Domain.Products
{
    public class ProductPhoto
    {
        public Guid Id { get; set; }

        [Required]
        public string PhotoId { get; set; }

        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
