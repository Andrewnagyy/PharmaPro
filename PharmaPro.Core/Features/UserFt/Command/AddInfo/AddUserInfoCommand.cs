using MediatR;
using PharmaPro.Core.Contract.Api;

namespace PharmaPro.Core.Features.UserFt.Command.AddInfo
{
    public class AddUserInfoCommand : IRequest<APIResponse<AddUserInfoCommandResponse>>
    {
        public string Name { get; set; }
        public string Age { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public ICollection<string> PhoneNumber { get; set; }
        public ICollection<string> ChronicDisease { get; set; }
    }

}
