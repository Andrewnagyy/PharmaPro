using MediatR;

namespace PharmaPro.Core.Features.Storageft.Query
{
    public class GetImageQuery : IRequest<GetImageQueryResponse>
    {
        public Guid Id { get; set; }
    }
}
