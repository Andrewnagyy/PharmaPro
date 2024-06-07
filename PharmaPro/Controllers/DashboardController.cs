using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmaPro.Core.Features.CategorysFT.Command.AddCategory;
using PharmaPro.Core.Features.OrderFt.Query.GetDaily;
using PharmaPro.Core.Features.OrderFt.Query.GetDailySales;
using PharmaPro.Core.Features.OrderFt.Query.GetMonthly;

namespace PharmaPro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : BaseController
    {
        private readonly IMediator _mediatR;

        public DashboardController(IMediator mediator)
        {
            _mediatR = mediator;
        }

        [HttpGet("GetDaily")]
        public async Task<ActionResult<GetDailySalesQuery>> GetDaily()
        {
            var result = await _mediatR.Send(new GetDailySalesQuery());
            return GetApiResponse(result);
        }

        [HttpGet("GetMonthly")]
        public async Task<ActionResult<GetMonthlySalesQuery>> GetMonthly()
        {
            var result = await _mediatR.Send(new GetDailySalesQuery());
            return GetApiResponse(result);
        }


    }
}
