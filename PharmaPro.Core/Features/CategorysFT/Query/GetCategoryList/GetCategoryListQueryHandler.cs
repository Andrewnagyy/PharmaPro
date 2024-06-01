using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmaPro.Core.Contract.Api;
using PharmaPro.DS;
using System.Net;

namespace PharmaPro.Core.Features.CategorysFT.Query.GetCategoryList
{
    public record CategoryDto(Guid Id, string? Name);
    public class GetCategoryListQueryHandler : IRequestHandler<GetCategoryListQuery, APIResponse<GetCategoryListQueryResponse>>
    {
        private readonly AppDbContext _dbContext;
        public GetCategoryListQueryHandler(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;

        }
        public async Task<APIResponse<GetCategoryListQueryResponse>> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
        {
            var categories = await _dbContext.categories
                .ToListAsync();

            if (categories == null || !categories.Any())
            {
                return new APIResponse<GetCategoryListQueryResponse>
                {
                    Errors = new List<string>
                    {
                        "No Categories found!"
                    },
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            var response = new GetCategoryListQueryResponse(
                categories.Select(l => new CategoryDto(
                    l.Id,
                    l.Name
                )).ToList()
            );

            return new APIResponse<GetCategoryListQueryResponse>
            {
                Data = response,
                HttpStatusCode = HttpStatusCode.OK
            };
        }
    }
}
