using MediatR;
using Microsoft.AspNetCore.Identity;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Domain.Users;
using PharmaPro.DS;
using System.Net;

namespace PharmaPro.Core.Features.UserFt.Command.AddInfo
{
    public class AddUserInfoCommandHandler : IRequestHandler<AddUserInfoCommand, APIResponse<AddUserInfoCommandResponse>>
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public AddUserInfoCommandHandler(AppDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<APIResponse<AddUserInfoCommandResponse>> Handle(AddUserInfoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.userId.ToString());

                if (user == null)
                {
                    return new APIResponse<AddUserInfoCommandResponse>
                    {
                        Data = new AddUserInfoCommandResponse(),
                        HttpStatusCode = HttpStatusCode.NotFound
                    };
                }

                string userEmail = user.Email;

                var newUser = new User
                {
                    UserID = request.userId,
                    Name = request.Name,
                    Gmail = userEmail,
                    City = request.City,
                    Street = request.Street,
                    PhoneNumber = request.PhoneNumber,
                    Age = request.Age,
                    ChronicDisease = request.ChronicDisease
                };

                _dbContext.users.Add(newUser);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return new APIResponse<AddUserInfoCommandResponse>
                {
                    Data = new AddUserInfoCommandResponse
                    {
                        Message = "Profile has been Completed"
                    },
                    HttpStatusCode = HttpStatusCode.Created
                };
            }
            catch (Exception ex)
            {
                // Log the exception
                return new APIResponse<AddUserInfoCommandResponse>
                {
                    Data = null,
                    HttpStatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
    }
}
