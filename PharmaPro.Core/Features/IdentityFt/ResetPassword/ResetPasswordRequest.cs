using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaPro.Core.Features.IdentityFt.ResetPassword
{
    public class ResetPasswordRequest
    {
        public string Token { get; set; }
        public string Email { get; set; }
        
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
