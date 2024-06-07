using MediatR;
using Microsoft.AspNetCore.Http;
using PharmaPro.Core.Contract.Api;

namespace PharmaPro.Core.Features.ProductFT.Command.EditProduct
{
    public class EditProductCommand : IRequest<APIResponse<EditProductCommandResponse>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public string BarCode { get; set; }
        public bool Active { get; set; }
        public bool SoldOut { get; set; }
        public DateTime ExpirationDate { get; set; }

        public decimal Price { get; set; }
        public Guid CategoryId { get; set; }
        public IFormFile PhotoFile { get; set; }

    }
}
