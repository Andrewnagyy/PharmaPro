using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaPro.Core.Features.ProductFT.Query.GetProductById
{
    public class GetProductByIdQueryResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Photo { get; set; }
        public int Amount { get; set; }
        public string BarCode { get; set; }
        public bool Active { get; set; }
        public bool SoldOut { get; set; }
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; }
    }
}
