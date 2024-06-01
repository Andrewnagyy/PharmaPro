using MediatR;
using PharmaPro.DS;

namespace PharmaPro.Core.Features.Storageft.Query
{
    public class GetImageQueryHandler : IRequestHandler<GetImageQuery, GetImageQueryResponse>
    {
        private readonly AppDbContext _dbContext;

        public GetImageQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<GetImageQueryResponse> Handle(GetImageQuery request, CancellationToken cancellationToken)
        {
            var imageEntity = _dbContext.ImagesStorage.FirstOrDefault(x => x.Id == request.Id);

            if (imageEntity == null)
                return Task.FromResult<GetImageQueryResponse>(null);

            var imageModel = new GetImageQueryResponse
            {
                Id = imageEntity.Id,
                ImageString = imageEntity.ImageReference
            };

            return Task.FromResult(imageModel);
        }

    }
}
