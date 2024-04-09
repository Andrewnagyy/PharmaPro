using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Core.Features.ProductFT.Query.GetProductList;
using PharmaPro.Domain.Users;
using PharmaPro.DS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PharmaPro.Core.Features.UserFt.Query.GetUserList
{
    public record UserDto(Guid ID, string Name, string Email,string PhoneNumber ,string city, string Street,string ChronicDisease);
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
                .Include(p => p.PhoneNumber)
                .Include(p => p.ChronicDisease)
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
                    p.Email,
                    p.PhoneNumber.FirstOrDefault()?.PhoneNumber,
                    p.City,
                    p.Street,
                    p.ChronicDisease.FirstOrDefault()?.ChronicDisease
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