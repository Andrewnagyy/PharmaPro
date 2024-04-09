using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaPro.Core.Features.UserFt.Query.GetUserList
{
   public record GetUserListResponse(List<UserDto> Users);
}
