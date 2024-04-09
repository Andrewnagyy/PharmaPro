using MediatR;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Core.Contract.Identity;
using PharmaPro.Domain.Users;
using PharmaPro.DS;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace PharmaPro.Core.Features.UserFt.Command.AddInfo
{
    public class AddUserInfoCommandHandler : IRequestHandler<AddUserInfoCommand, APIResponse<AddUserInfoCommandResponse>>
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserToken _userToken;
        private readonly UserManager<IdentityUser> _userManager;

        public AddUserInfoCommandHandler(AppDbContext appDbContext, IUserToken userToken, UserManager<IdentityUser> userManager)
        {
            _dbContext = appDbContext;
            _userToken = userToken;
            _userManager = userManager;
        }

        public async Task<APIResponse<AddUserInfoCommandResponse>> Handle(AddUserInfoCommand request, CancellationToken cancellationToken)
        {
            Guid userId = await _userToken.GetUserIDFromToken();
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                // Handle the case where user is not found
                return new APIResponse<AddUserInfoCommandResponse>
                {
                    Data = new AddUserInfoCommandResponse(),
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            string userEmail = user.Email;

            var phoneNumbers = request.PhoneNumber.Select(p => new PhoneNumbers { PhoneNumber = p }).ToList();
            var chronicDiseases = request.ChronicDisease.Select(d => new ChronicDiseases { ChronicDisease = d }).ToList();

            var newUser = new User
            {
                UserID = userId,
                Email = userEmail,
                Name = request.Name,
                City = request.City,
                Street = request.Street,
                PhoneNumber = phoneNumbers,
                Age = request.Age,
                ChronicDisease = chronicDiseases
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
    }
}
