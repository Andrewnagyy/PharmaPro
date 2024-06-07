using MediatR;
using Microsoft.AspNetCore.Http;
using PharmaPro.Core.Contract.Api;
using System.ComponentModel.DataAnnotations;

namespace PharmaPro.Core.Features.ProductFT.Command.AddProduct
{
    public class AddProductCommand : IRequest<APIResponse<AddProductCommandResponse>>
    {
        [Required, MinLength(2), MaxLength(50)]
        public string Name { get; set; }

        [Required, MinLength(1), MaxLength(150)]
        public string Description { get; set; }
        public int Amount { get; set; }

        [MinLength(6), MaxLength(14)]
        public string BarCode { get; set; }
        public bool Active { get; set; }
        public bool SoldOut { get; set; }
        public DateTime ExpirationDate { get; set; }

        public decimal Price { get; set; }

        public Guid CategoryId { get; set; }
        public IFormFile PhotoFile { get; set; }
    }
}
