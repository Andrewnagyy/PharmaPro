using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PharmaPro.Controllers;
using PharmaPro.Core.Features.IdentityFt.Login.Command;
using PharmaPro.Core.Features.IdentityFt.Register.Command;
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
  

        public UserController(IAuthorizationRepository authService,IMediator mediator)
        {
            _mediatR = mediator;
            _authService = authService;
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
    }
}