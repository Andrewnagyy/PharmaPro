using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaPro.Core.Features.CategorysFT.Query.GetCategoryList
{
    public record GetCategoryListQueryResponse(List<CategoryDto> Categories);
}
