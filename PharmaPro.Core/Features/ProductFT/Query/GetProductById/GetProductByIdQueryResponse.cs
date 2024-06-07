namespace PharmaPro.Core.Features.ProductFT.Query.GetProductById
{
    public class GetProductByIdQueryResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public int Amount { get; set; }
        public string BarCode { get; set; }
        public bool Active { get; set; }
        public bool SoldOut { get; set; }
        public DateTime ExpirationDate { get; set; }

        public decimal Price { get; set; }
        public bool Offer { get; set; }
        public int Discount { get; set; }
        public decimal OldPrice { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
