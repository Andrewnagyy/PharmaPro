using MediatR;
using PharmaPro.Core.Contract.Api;

namespace PharmaPro.Core.Features.UserFt.Command.AddInfo
{
    public class AddUserInfoCommand : IRequest<APIResponse<AddUserInfoCommandResponse>>
    {
        public Guid userId { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PhoneNumber { get; set; }
        public string ChronicDisease { get; set; }
    }

}
