using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg;
using PharmaPro.Core.Contract;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Core.Features.IdentityFt.isBlocked.blockUser;
using PharmaPro.Core.Features.IdentityFt.Login.Command;
using PharmaPro.Core.Features.IdentityFt.Register.Command;
using PharmaPro.Core.Features.IdentityFt.ResetPassword;
using PharmaPro.Core.Features.UserFt.Command.AddInfo;
using PharmaPro.Core.Features.UserFt.Query.GetUserList;
using PharmaPro.Repositories.AuthorizationRepo;
using System.Net;
using static PharmaPro.Repositories.AuthorizationRepo.AuthorizationRepository;
using static PharmaPro.SendGrid.Service.EmailSenderService;

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
        private readonly IEmailSender _emailSender;


        public UserController(IAuthorizationRepository authService, IMediator mediator, UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _mediatR = mediator;
            _authService = authService;
            _userManager = userManager;
            _emailSender = emailSender;
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
                id = pharmacist.Id,
                Email = pharmacist.Email,
                Username = pharmacist.UserName
            }).ToList();

            return Ok(pharmacistInfo);
        }

        [HttpGet("GetUsersList")]
        [EnableCors("ReactPolicy")]
        public async Task<ActionResult<GetUserListQuery>> GetUsersList()
        {
            var result = await _mediatR.Send(new GetUserListQuery());
            return GetApiResponse(result);
        }

        [HttpPost("BlockUser")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BlockUser([FromQuery] BlockUserCommand command)
        {
            var response = await _mediatR.Send(command);

            if (response.Errors != null && response.Errors.Count > 0)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("unBlockUser")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> unBlockUser([FromQuery] unBlockUserCommand command)
        {
            var response = await _mediatR.Send(command);

            if (response.Errors != null && response.Errors.Count > 0)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("requestResetPassword")]
        public async Task<IActionResult> RequestResetPassword([FromQuery] string email)
        {
            var response = await _authService.RequestPasswordReset(email);
            if (response.Errors != null && response.Errors.Count > 0)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var response = await _authService.ResetPassword(request.Email, request.Token, request.NewPassword, request.ConfirmPassword);
            if (response.Errors != null && response.Errors.Count > 0)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpDelete("deleteUser")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await _authService.DeleteUserAccountAsync(id);

            if (result.HttpStatusCode == HttpStatusCode.OK)
            {
                return Ok(result.Data);
            }

            return StatusCode((int)result.HttpStatusCode, result.Errors);
        }
    }
}