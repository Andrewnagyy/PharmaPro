using PharmaPro.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaPro.Domain.Categories
{
    public class Category
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}
