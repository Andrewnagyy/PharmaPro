using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Domain.Categories;
using PharmaPro.DS;
using PharmaPro.DS.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PharmaPro.Core.Features.CategorysFT.Query.GetCategoryByID
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, APIResponse<GetCategoryByIdQueryResponse>>
    {
        private readonly AppDbContext _dbContext;
        public GetCategoryByIdQueryHandler(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;

        }
        public async Task<APIResponse<GetCategoryByIdQueryResponse>> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
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
