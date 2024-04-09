using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmaPro.Core.Features.CategorysFT.Command.AddCategory;
using PharmaPro.Core.Features.CategorysFT.Command.DeleteCategory;
using PharmaPro.Core.Features.CategorysFT.Command.EditCategory;
using PharmaPro.Core.Features.CategorysFT.Query.GetCategoryByID;
using PharmaPro.Core.Features.CategorysFT.Query.GetCategoryList;
using PharmaPro.Core.Features.ProductFT.Command.AddProduct;
using PharmaPro.Core.Features.ProductFT.Command.DeleteProduct;
using PharmaPro.Core.Features.ProductFT.Command.EditProduct;
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
        public async Task<ActionResult<AddProductCommand>> AddProduct([FromBody] AddProductCommand command)
        {
            var result = await _mediatR.Send(command);
            return GetApiResponse(result);
        }

        [HttpPut("UpdateProduct")]
        public async Task<ActionResult<EditProductCommand>> UpdateCategory(EditProductCommand command)
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
        public async Task<ActionResult<GetProductByIdQuery>> ProductById([FromQuery] GetProductByIdQuery query)
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
