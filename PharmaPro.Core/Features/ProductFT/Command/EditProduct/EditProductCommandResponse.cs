namespace PharmaPro.Core.Features.ProductFT.Command.EditProduct
{
    public class EditProductCommandResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public int Amount { get; set; }
        public string BarCode { get; set; }
        public bool IsActive { get; set; }
        public bool IsSoldOut { get; set; }
        public DateTime ExpirationDate { get; set; }

        public decimal Price { get; set; }
        public Guid CategoryId { get; set; }
        public string Message { get; set; }
    }
}
