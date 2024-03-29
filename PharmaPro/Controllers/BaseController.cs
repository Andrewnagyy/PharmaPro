using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PharmaPro.Core.Contract.Api;

namespace PharmaPro.Controllers
{
    [ApiController]
    [EnableCors("ReactPolicy")]
    public class BaseController : ControllerBase
    {
        protected ActionResult GetApiResponse<T>(APIResponse<T> obj)
        {
            if (obj.IsSuccessStatusCode)
                return StatusCode(obj.StatusCode, obj.Data);

            return StatusCode(obj.StatusCode, obj.Errors);
        }
    }
}