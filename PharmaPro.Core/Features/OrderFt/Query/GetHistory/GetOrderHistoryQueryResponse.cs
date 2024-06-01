namespace PharmaPro.Core.Features.OrderFt.Query.GetHistory
{
    public class GetOrderHistoryQueryResponse
    {
        public List<OrderHistoryItem> OrderHistory { get; set; }

        public class OrderHistoryItem
        {
            public Guid OrderId { get; set; }
            public DateTime CreatedAt { get; set; }
            public decimal TotalPrice { get; set; }
            public bool OrderIsDone { get; set; }
            public List<OrderProduct> orderProducts { get; set; }
        }

        public class OrderProduct
        {
            public Guid ProductId { get; set; }
            public string productName { get; set; }
            public decimal productPrice { get; set; }
            public int Amount { get; set; }
        }
    }
}