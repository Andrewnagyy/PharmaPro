using PharmaPro.Domain.Categories;
using PharmaPro.Domain.Orders;
using PharmaPro.Domain.UserProducts;

namespace PharmaPro.Domain.Products
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public int Amount { get; set; }
        public string BarCode { get; set; }
        public bool Active { get; set; }
        public bool SoldOut { get; set; }
        public DateTime ExpirationDate { get; set; }
        public decimal Price { get; set; } = 18.20M;
        public bool Offer { get; set; }
        public int Discount { get; set; }
        public decimal OldPrice { get; set; }


        // Foreign key property
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<OrderProducts> OrderProducts { get; set; }

        public ICollection<UserProduct> UserProducts { get; set; }


    }
}
