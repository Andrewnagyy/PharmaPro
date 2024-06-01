using MediatR;
using PharmaPro.Core.Contract.Api;

namespace PharmaPro.Core.Features.UserFt.Query.GetUserList
{
    public record GetUserListQuery : IRequest<APIResponse<GetUserListResponse>>;
}
