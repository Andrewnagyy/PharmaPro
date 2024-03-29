using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmaPro.Controllers;
using PharmaPro.Core.Features.CategorysFT.Command.AddCategory;
using PharmaPro.Core.Features.CategorysFT.Command.DeleteCategory;
using PharmaPro.Core.Features.CategorysFT.Command.EditCategory;
using PharmaPro.Core.Features.CategorysFT.Query.GetCategoryByID;
using PharmaPro.Core.Features.CategorysFT.Query.GetCategoryList;

namespace PharmaPure.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class CategoryController : BaseController
    {
        private readonly IMediator _mediatR;

        public CategoryController(IMediator mediator)
        {
            _mediatR = mediator;
        }

        [HttpPost("AddCategory")]
        public async Task<ActionResult<AddCategoryCommand>> AddCustomer([FromBody] AddCategoryCommand command)
        {
            var result = await _mediatR.Send(command);
            return GetApiResponse(result);
        }

        [HttpPut("UpdateCategory")]
        public async Task<ActionResult<EditCategoryCommand>> UpdateCategory(EditCategoryCommand command)
        {
            var result = await _mediatR.Send(command);
            return GetApiResponse(result);
        }

        [HttpDelete("DeleteCategory")]
        public async Task<ActionResult<DeleteCategoryCommandResponse>> DeleteCategory([FromQuery] DeleteCategoryCommand command)
        {
            var result = await _mediatR.Send(command);
            return GetApiResponse(result);
        }

        [HttpGet("GetCategoryByID")]
        public async Task<ActionResult<GetCategoryByIdQuery>> CategoryById([FromQuery] GetCategoryByIdQuery query)
        {
            var result = await _mediatR.Send(query);
            return GetApiResponse(result);
        }

        [HttpGet("GetCategoryList")]
        public async Task<ActionResult<GetCategoryListQueryResponse>> CategotyList()
        {
            var result = await _mediatR.Send(new GetCategoryListQuery());
            return GetApiResponse(result);
        }


    }
}
