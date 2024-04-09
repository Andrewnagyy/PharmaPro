using MediatR;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Domain.Categories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaPro.Core.Features.ProductFT.Command.AddProduct
{
    public class AddProductCommand : IRequest<APIResponse<AddProductCommandResponse>>
    {
        [Required, MinLength(2), MaxLength(50)]
        public string Name { get; set; }

        [Required, MinLength(1), MaxLength(150)]
        public string Description { get; set; }
        public List<string> Photos { get; set; }
        public int Amount { get; set; }

        [MinLength(6), MaxLength(14)]
        public string BarCode { get; set; }
        public bool Active { get; set; }
        public bool SoldOut { get; set; }
        public decimal Price { get; set; }

        public Guid CategoryId { get; set; }
    }
}
