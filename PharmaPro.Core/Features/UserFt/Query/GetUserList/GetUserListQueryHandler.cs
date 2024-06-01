using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmaPro.Core.Contract.Api;
using PharmaPro.DS;
using System.Net;

namespace PharmaPro.Core.Features.UserFt.Query.GetUserList
{
    public record UserDto(Guid ID, string Name, string Gmail, string PhoneNumber, string city, string Street, string ChronicDisease);
    public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, APIResponse<GetUserListResponse>>
    {

        private readonly AppDbContext _dbContext;
        public GetUserListQueryHandler(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task<APIResponse<GetUserListResponse>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var users = await _dbContext.users
                .ToListAsync(cancellationToken);


            if (users == null || !users.Any())
            {
                return new APIResponse<GetUserListResponse>
                {
                    Errors = new List<string> { "There are no Users!!" },
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            var response = new GetUserListResponse(
                users.Select(p => new UserDto(
                    p.UserID,
                    p.Name,
                    p.Gmail,
                    p.PhoneNumber,
                    p.City,
                    p.Street,
                    p.ChronicDisease
                )).ToList()
            );

            return new APIResponse<GetUserListResponse>
            {
                Data = response,
                HttpStatusCode = HttpStatusCode.OK
            };
        }
    }
}