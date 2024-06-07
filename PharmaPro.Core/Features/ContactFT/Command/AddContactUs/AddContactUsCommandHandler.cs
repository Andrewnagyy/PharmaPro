using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Core.Features.CategorysFT.Command.AddCategory;
using PharmaPro.Domain.Categories;
using PharmaPro.Domain.Contacts;
using PharmaPro.DS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PharmaPro.Core.Features.ContactFT.Command.AddContactUs
{
    public class AddContactUsCommandHandler : IRequestHandler<AddContactUsCommand, APIResponse<AddContactUsCommandResponse>>
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        public AddContactUsCommandHandler(AppDbContext appDbContext,UserManager<IdentityUser> userManager)
        {
            _dbContext = appDbContext;
            _userManager = userManager;
            
        }
        public async Task<APIResponse<AddContactUsCommandResponse>> Handle(AddContactUsCommand request, CancellationToken cancellationToken)
        {
            var emailExists = await _userManager.FindByEmailAsync(request.Email);
            if (emailExists == null)
            {
                return new APIResponse<AddContactUsCommandResponse>
                {
                    Errors = new List<string>()
                    {
                        "This email not found"
                    },
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            var contactUs = new ContactUs
            {
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Message = request.Message,
            };

            _dbContext.ContactUs.Add(contactUs);
            await _dbContext.SaveChangesAsync(cancellationToken);


            return new APIResponse<AddContactUsCommandResponse>
            {
                Data = new AddContactUsCommandResponse()
                {
                    Message = "sent Successfully"
                },
                HttpStatusCode = HttpStatusCode.Created
            };

        }
    }
}
