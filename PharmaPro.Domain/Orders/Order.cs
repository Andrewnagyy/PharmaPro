using PharmaPro.Domain.Products;

namespace PharmaPro.Domain.Orders
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public decimal TotalPrice { get; set; }
        public ICollection<OrderProducts> OrderProducts { get; set; }
        public bool OrderIsDone { get; set; }
    }

    public class OrderProducts
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }
        public Guid OrderId { get; set; }
        public int Amount { get; set; }

    }
}