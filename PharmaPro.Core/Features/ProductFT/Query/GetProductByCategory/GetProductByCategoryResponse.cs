using PharmaPro.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaPro.Core.Features.ProductFT.Query.GetProductByCategory
{
    public class GetProductByCategoryResponse
    {
        public List<Product> Products { get; set; }
    }
}
