using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmaPro.Core.Contract.Api;
using PharmaPro.DS;

namespace PharmaPro.Core.Features.CategorysFT.Query.GetCategoryByID
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetproductByIdQuery, APIResponse<GetCategoryByIdQueryResponse>>
    {
        private readonly AppDbContext _dbContext;
        public GetCategoryByIdQueryHandler(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;

        }
        public async Task<APIResponse<GetCategoryByIdQueryResponse>> Handle(GetproductByIdQuery query, CancellationToken cancellationToken)
        {
            bool categoryExists = await _dbContext.categories.AnyAsync(c => c.Id == query.CategoryId, cancellationToken);
            if (!categoryExists)
            {
                return new APIResponse<GetCategoryByIdQueryResponse>()
                {
                    Errors = new List<string>()
                {
                    "Category is Not Found!"
                },
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound,
                };
            }
            else
            {
                var category = await _dbContext.categories.FindAsync(query.CategoryId);

                return new APIResponse<GetCategoryByIdQueryResponse>()
                {
                    Data = new GetCategoryByIdQueryResponse()
                    {
                        Id = category.Id,
                        Name = category.Name,
                    },
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                };
            }
        }
    }
}
