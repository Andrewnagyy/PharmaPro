using MediatR;
using PharmaPro.Core.Contract.Api;
using System.Collections.Generic;

namespace PharmaPro.Core.Features.ContactFT.Query.GetContactUs
{
    public record GetContactUsListQuery : IRequest<APIResponse<List<ContactsDto>>>;
}
