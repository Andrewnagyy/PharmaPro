using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaPro.Core.Features.ProductFT.Command.AddOffer;
using PharmaPro.Core.Features.ProductFT.Command.AddProduct;
using PharmaPro.Core.Features.ProductFT.Command.DeleteProduct;
using PharmaPro.Core.Features.ProductFT.Command.EditProduct;
using PharmaPro.Core.Features.ProductFT.Query.GetProductByCategory;
using PharmaPro.Core.Features.ProductFT.Query.GetProductById;
using PharmaPro.Core.Features.ProductFT.Query.GetProductList;

namespace PharmaPro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController
    {
        private readonly IMediator _mediatR;

        public ProductController(IMediator mediator)
        {
            _mediatR = mediator;
        }

        [HttpPost("AddProduct")]
        public async Task<ActionResult<AddProductCommand>> AddProduct([FromForm] AddProductCommand command)
        {
            var result = await _mediatR.Send(command);
            return GetApiResponse(result);
        }

        [HttpPut("UpdateProduct")]
        public async Task<ActionResult<EditProductCommand>> UpdateProduct([FromForm] EditProductCommand command)
        {
            var result = await _mediatR.Send(command);
            return GetApiResponse(result);
        }

        [HttpPut("AddOffer")]
        public async Task<ActionResult<AddOfferCommand>> AddOffer(AddOfferCommand command)
        {
            var result = await _mediatR.Send(command);
            return GetApiResponse(result);
        }

        [HttpDelete("DeleteProduct")]
        public async Task<ActionResult<DeleteProductCommandResponse>> DeleteProduct([FromQuery] DeleteProductCommand command)
        {
            var result = await _mediatR.Send(command);
            return GetApiResponse(result);
        }

        [HttpGet("GetProductByID")]
        //[Authorize]
        public async Task<ActionResult<GetProductByIdQuery>> ProductById([FromQuery] GetProductByIdQuery query)
        {
            var result = await _mediatR.Send(query);
            return GetApiResponse(result);
        }

        [HttpGet("GetProductByCategoryID")]
        public async Task<ActionResult<GetProductByCategoryQuery>> GetProductByCategoryID([FromQuery] GetProductByCategoryQuery query)
        {
            var result = await _mediatR.Send(query);
            return GetApiResponse(result);
        }

        [HttpGet("GetProductList")]
        public async Task<ActionResult<GetProductListQueryResponse>> ProductList()
        {
            var result = await _mediatR.Send(new GetProductListQuery());
            return GetApiResponse(result);
        }

    }
}
