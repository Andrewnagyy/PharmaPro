using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmaPro.Controllers;

namespace PharmaPure.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("ReactPolicy")]
    [ApiController]
    public class CustomerController : BaseController
    {
        private readonly IMediator _mediatR;

        public CustomerController(IMediator mediator)
        {
            _mediatR = mediator;
        }

     /*   [HttpPost("AddCustomer")]
        public async Task<ActionResult<AddCustomerCommand>> AddCustomer([FromBody] AddCustomerCommand command)
        {
            var result = await _mediatR.Send(command);
            return GetApiResponse(result);
        }
     */
    }
}
