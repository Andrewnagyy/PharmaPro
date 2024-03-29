using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Core.Features.CategorysFT.Command.DeleteCategory;
using PharmaPro.DS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PharmaPro.Core.Features.CategorysFT.Command.EditCategory
{
    public class EditCategoryCommandHandler : IRequestHandler<EditCategoryCommand, APIResponse<EditCategoryCommandResponse>>
    {
        private readonly AppDbContext _dbContext;

        public EditCategoryCommandHandler(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task<APIResponse<EditCategoryCommandResponse>> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _dbContext.categories.FindAsync(request.CategoryId);
            if (category == null)
            {
                return new APIResponse<EditCategoryCommandResponse>
                {
                    Errors = new List<string> { "Category not found" },
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            category.Name = request.NewName;
            _dbContext.categories.Update(category);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new APIResponse<EditCategoryCommandResponse>
            {
                Data = new EditCategoryCommandResponse()
                {
                    Message = "Category successfully Updated",
                },
                HttpStatusCode = HttpStatusCode.OK,
            };
        }
    }
}
