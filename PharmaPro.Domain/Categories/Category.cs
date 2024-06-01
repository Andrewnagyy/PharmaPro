using PharmaPro.Domain.Products;

namespace PharmaPro.Domain.Categories
{
    public class Category
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}
