using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmaPro.Core.Features.CategorysFT.Command.AddCategory;
using PharmaPro.Core.Features.CategorysFT.Query.GetCategoryList;
using PharmaPro.Core.Features.ContactFT.Command.AddContactUs;
using PharmaPro.Core.Features.ContactFT.Query.GetContactUs;

namespace PharmaPro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : BaseController
    {
        private readonly IMediator _mediatR;

        public ContactUsController(IMediator mediator)
        {
            _mediatR = mediator;
        }

        [HttpPost("ContactUs")]
        public async Task<ActionResult<AddContactUsCommand>> ContactUs([FromBody] AddContactUsCommand command)
        {
            var result = await _mediatR.Send(command);
            return GetApiResponse(result);
        }

        [HttpGet("GetMessageList")]
        public async Task<ActionResult<GetContactUsListQuery>> GetMessageList()
        {
            var result = await _mediatR.Send(new GetContactUsListQuery());
            return GetApiResponse(result);
        }

    }
}
