using MediatR;
using PharmaPro.Core.Contract.Api;
using PharmaPro.Core.Features.Storageft.Command.UploadImage;
using PharmaPro.Core.Helpers;
using PharmaPro.Domain.Storage;
using PharmaPro.DS;
using System.Diagnostics;
using System.Net;

namespace PharmaPro.Core.Features.Storageft.Command.UploadImage
{
    public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, APIResponse<UploadImageCommandResponse>>
    {
        private readonly string _stoaragePath;
        private readonly AppDbContext _dbContext;

        public UploadImageCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _stoaragePath = Path.Combine(Globals.StorageRootPath, Globals.UploadPath);
        }

        public async Task<APIResponse<UploadImageCommandResponse>> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            if (!request.file.ContentType.StartsWith("image/"))
            {
                return new APIResponse<UploadImageCommandResponse>()
                {
                    Errors = new List<string>()
                    {
                        $"Content Type '{request.file.ContentType}' Not Supported here, Only Images are allowed"
                    },
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }

            if (!Directory.Exists(_stoaragePath))
                Directory.CreateDirectory(_stoaragePath);

            string[] fileSplit = request.file.FileName.Split('.');
            if (!AllowedExtensions.Get().Contains(fileSplit.Last().ToUpper()))
            {
                return new APIResponse<UploadImageCommandResponse>()
                {
                    Errors = new List<string>()
                    {
                        $"The {fileSplit.Last()} is not allowed in this website!"
                    },
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };
            }

            string fileStoredName = $"{fileSplit.FirstOrDefault()}_{Guid.NewGuid()}.{fileSplit.LastOrDefault()}";

            String FullPath = Path.Combine(_stoaragePath, fileStoredName);

            using (var stream = new FileStream(FullPath, FileMode.Create))
            {
                await request.file.CopyToAsync(stream);
            }

            var doc = new ImageStorage()
            {
                Id = Guid.NewGuid(),
                ImageReference = fileStoredName
            };

            await _dbContext.ImagesStorage.AddAsync(doc);
            await _dbContext.SaveChangesAsync();
            stopwatch.Stop();
            var ell = stopwatch.ElapsedMilliseconds;

            return new APIResponse<UploadImageCommandResponse>()
            {
                Data = new UploadImageCommandResponse(doc.Id),
                HttpStatusCode = HttpStatusCode.OK,
            };
        }
    }
}