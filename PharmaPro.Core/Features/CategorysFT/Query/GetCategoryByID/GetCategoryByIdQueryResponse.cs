using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaPro.Core.Features.CategorysFT.Query.GetCategoryByID
{
    public class GetCategoryByIdQueryResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}
