using MediatR;
using PharmaPro.Core.Contract.Api;
using PharmaPro.DS;
using System.Net;

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
            var category = await _dbContext.categories.FindAsync(request.Id);
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
