using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Core.Features.CategorysFT.Command.AddCategory;
using PharmaPro.Domain.Categories;
using PharmaPro.DS;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PharmaPro.Core.Features.CategorysFT.Command.AddCategory
{
    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, APIResponse<AddCategoryCommandResponse>>
    {

        private readonly AppDbContext _dbContext;

        public AddCategoryCommandHandler(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }
        public async Task<APIResponse<AddCategoryCommandResponse>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {

            bool categoryExists = await _dbContext.categories.AnyAsync(c => c.Name == request.Name, cancellationToken);
            if (categoryExists)
            {
                return new APIResponse<AddCategoryCommandResponse>
                {
                    Errors = new List<string>()
                    {
                        "This Category Name is already Used"
                    },
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            var category = new Category
            {
                Name = request.Name,
            };

            _dbContext.categories.Add(category);
            await _dbContext.SaveChangesAsync(cancellationToken);


            return new APIResponse<AddCategoryCommandResponse>
            {
                Data = new AddCategoryCommandResponse()
                {
                    Id = category.Id,
                },
                HttpStatusCode = HttpStatusCode.Created
            };

        }
    }
}
