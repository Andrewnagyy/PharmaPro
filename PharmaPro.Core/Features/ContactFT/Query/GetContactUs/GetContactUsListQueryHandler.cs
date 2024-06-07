using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmaPro.Core.Contract.Api;
using PharmaPro.DS;
using System.Net;

namespace PharmaPro.Core.Features.ContactFT.Query.GetContactUs
{
    public record ContactsDto(Guid Id, string Name, string Message);

    public class GetContactUsListQueryHandler : IRequestHandler<GetContactUsListQuery, APIResponse<List<ContactsDto>>>
    {
        private readonly AppDbContext _dbContext;

        public GetContactUsListQueryHandler(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task<APIResponse<List<ContactsDto>>> Handle(GetContactUsListQuery request, CancellationToken cancellationToken)
        {
            var contactsus = await _dbContext.ContactUs
                .ToListAsync();

            if (contactsus == null || !contactsus.Any())
            {
                return new APIResponse<List<ContactsDto>>
                {
                    Errors = new List<string>
                    {
                        "No Messages found!"
                    },
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            var response = contactsus.Select(l => new ContactsDto(
                l.Id,
                l.Name,
                l.Message
            )).ToList();

            return new APIResponse<List<ContactsDto>>
            {
                Data = response,
                HttpStatusCode = HttpStatusCode.OK
            };
        }
    }
}
