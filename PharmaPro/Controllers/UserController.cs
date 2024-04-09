using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmaPro.Controllers;
using PharmaPro.Core.Features.CategorysFT.Command.AddCategory;
using PharmaPro.Core.Features.IdentityFt.Login.Command;
using PharmaPro.Core.Features.IdentityFt.Register.Command;
using PharmaPro.Core.Features.ProductFT.Query.GetProductList;
using PharmaPro.Core.Features.UserFt.Command.AddInfo;
using PharmaPro.Core.Features.UserFt.Query.GetUserList;
using PharmaPro.Repositories.AuthorizationRepo;
using System.Net;
using System.Security.Claims;

namespace PharmaPro.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("ReactPolicy")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IAuthorizationRepository _authService;
        private readonly IMediator _mediatR;
        private readonly UserManager<IdentityUser> _userManager;


        public UserController(IAuthorizationRepository authService,IMediator mediator, UserManager<IdentityUser> userManager)
        {
            _mediatR = mediator;
            _authService = authService;
            _userManager = userManager;
        }


        [HttpPost("RegisterAsAdmin")]
        [EnableCors("ReactPolicy")]
        public async Task<IActionResult> RegisterAsAdmin(RegisterCommand command)
        {
            var response = await _authService.RegisterAdminAsync(command);

            if (response.Errors != null && response.Errors.Count > 0)
            {
                return BadRequest(response);
            }

            return Created(string.Empty, response);
        }

        [HttpPost("RegisterAsPharmacist")]
        [EnableCors("ReactPolicy")]
        public async Task<IActionResult> RegisterAsPharmacist(RegisterCommand command)
        {
            var response = await _authService.RegisterPharmacistAsync(command);

            if (response.Errors != null && response.Errors.Count > 0)
            {
                return BadRequest(response);
            }

            return Created(string.Empty, response);
        }

        [HttpPost("RegisterAsUser")]
        [EnableCors("ReactPolicy")]
        public async Task<IActionResult> RegisterAsUser(RegisterCommand command)
        {
            var response = await _authService.RegisterUserAsync(command);

            if (response.Errors != null && response.Errors.Count > 0)
            {
                return BadRequest(response);
            }

            return Created(string.Empty, response);
        }

        [HttpPost("CompleteRegisteration")]
        [Authorize]

        public async Task<ActionResult<AddUserInfoCommand>> CompleteRegisteration([FromBody] AddUserInfoCommand command)
        {
            var result = await _mediatR.Send(command);
            return GetApiResponse(result);
        }


        [HttpPost("Login")]
        [EnableCors("ReactPolicy")]
        public async Task<IActionResult> LoginUser(LoginCommand command)
        {
            var response = await _authService.LoginAsync(command);

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return Ok(response);
            }
            else if (response.HttpStatusCode == HttpStatusCode.Unauthorized)
            {
                return Unauthorized();
            }
            else
            {
                return BadRequest();
            }            
        }


        [HttpGet("GetAllPharmacist")]
        [EnableCors("ReactPolicy")]
        public async Task<IActionResult> GetAllPharmacist()
        {
            var pharmacists = await _userManager.GetUsersInRoleAsync("Pharmacist");

            if (pharmacists == null || !pharmacists.Any())
            {
                return NotFound("No pharmacists found");
            }
            var pharmacistInfo = pharmacists.Select(pharmacist => new
            {
                Email = pharmacist.Email,
                Username = pharmacist.UserName
            }).ToList();

            return Ok(pharmacistInfo);
        }

        [HttpGet("GetUsersList")]
        public async Task<ActionResult<GetProductListQueryResponse>> GetUsersList()
        {
            var result = await _mediatR.Send(new GetUserListQuery());
            return GetApiResponse(result);
        }

        
    }
}