using PharmaPro.Domain.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaPro.Domain.Products
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<ProductPhoto> Photo { get; set; }
        public int Amount { get; set; }
        public string BarCode { get; set; }
        public bool Active { get; set; }
        public bool SoldOut { get; set; }
        public decimal Price { get; set; }

        // Foreign key property
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
