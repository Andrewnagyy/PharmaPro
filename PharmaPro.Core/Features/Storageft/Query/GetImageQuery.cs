using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PharmaPro.Core.Features.Storageft.Query
{
    public class GetImageQuery : IRequest<GetImageQueryResponse>
    {
        public Guid Id { get; set; }
    }
}
