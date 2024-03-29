using MediatR;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Core.Features.CategorysFT.Command.AddCategory;
using PharmaPro.DS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PharmaPro.Core.Features.CategorysFT.Command.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, APIResponse<DeleteCategoryCommandResponse>>
    {
        private readonly AppDbContext _dbContext;

        public DeleteCategoryCommandHandler(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task<APIResponse<DeleteCategoryCommandResponse>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _dbContext.categories.FindAsync(request.CategoryId);
            if (category == null)
            {
                return new APIResponse<DeleteCategoryCommandResponse>
                {
                    Errors = new List<string> { "Category not found" },
                    HttpStatusCode = HttpStatusCode.NotFound
                };
            }

            _dbContext.categories.Remove(category);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new APIResponse<DeleteCategoryCommandResponse>
            {
                Data = new DeleteCategoryCommandResponse()
                {
                    Message = "Category successfully deleted",
                },
                HttpStatusCode = HttpStatusCode.OK
            };
        }
    }
}
