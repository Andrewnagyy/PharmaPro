using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaPro.Core.Features.OrderFt.Command.AddOrder;
using PharmaPro.Core.Features.OrderFt.Command.orderIsDone;
using PharmaPro.Core.Features.OrderFt.Query.GetHistory;
using PharmaPro.Core.Features.OrderFt.Query.GetOrdersList;

namespace PharmaPro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IMediator _mediatR;
        public OrderController(IMediator mediator)
        {
            _mediatR = mediator;
        }


        [HttpPost("AddOrder")]
        [Authorize]
        public async Task<ActionResult<AddOrderCommand>> AddCustomer([FromBody] AddOrderCommand command)
        {
            var result = await _mediatR.Send(command);
            return GetApiResponse(result);
        }

        [HttpPut("orderIsDone")]
        public async Task<IActionResult> OrderIsDone(Guid orderId)
        {
            var request = new OrderIsDoneCommand { OrderId = orderId };
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [HttpGet("OrdersList")]
        public async Task<IActionResult> GetOrderList()
        {
            var query = new GetOrderListQuery();
            var response = await _mediatR.Send(query);

            return GetApiResponse(response);
        }


        [HttpGet("GetOrdersHistory")]
        public async Task<ActionResult<GetOrderHistoryQuery>> GetOrdersHistory([FromQuery] GetOrderHistoryQuery query)
        {
            var result = await _mediatR.Send(query);
            return GetApiResponse(result);
        }


    }
}
