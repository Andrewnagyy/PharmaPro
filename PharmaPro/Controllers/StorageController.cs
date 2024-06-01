using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaPro.Core.Features.Storageft.Command.UploadImage;
using PharmaPro.Core.Features.Storageft.Query;
using PharmaPro.DS;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace PharmaPro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class StorageController : BaseController
    {
        private readonly IMediator _mediatR;
        private readonly AppDbContext _dbContext;

        public StorageController(IMediator mediator, AppDbContext appDbContext)
        {
            _mediatR = mediator;
            _dbContext = appDbContext;
        }

        [HttpPost("UploadImage")]
        // [Authorize]
        public async Task<ActionResult<UploadImageCommand>> UploadImage([FromForm] UploadImageCommand command)
        {
            var result = await _mediatR.Send(command);
            return GetApiResponse(result);
        }

        /* [HttpPost("UploadManyImages")]
         [Authorize]
         public async Task<ActionResult<List<Guid>>> UploadManyImages([FromForm] List<IFormFile> files)
         {
             var uploadedFileIds = new List<Guid>();

             foreach (var file in files)
             {
                 if (file.Length > 0)
                 {
                     var fileId = Guid.NewGuid();
                     var fileName = fileId.ToString() + Path.GetExtension(file.FileName);
                     var filePath = Path.Combine("Uploads", fileName);
                     using (var stream = new FileStream(filePath, FileMode.Create))
                     {
                         await file.CopyToAsync(stream);
                     }
                     uploadedFileIds.Add(fileId);

                     _dbContext.ImagesStorage.Add(new Domain.Storage.ImageStorage { Id = fileId, ImageReference = fileName });
                     await _dbContext.SaveChangesAsync();
                 }
             }

             return uploadedFileIds;
         }
        */

        [HttpGet("GetImageById")]
        public async Task<ActionResult<GetImageQueryResponse>> GetImage(Guid id)
        {
            var imageEntity = _dbContext.ImagesStorage.FirstOrDefault(i => i.Id == id);
            if (imageEntity == null)
                return NotFound();
            string basee = Directory.GetCurrentDirectory();
            string folder = Path.Combine(basee, "Uploads");

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string filePath = Path.Combine(folder, imageEntity.ImageReference);
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "image/jpeg");
        }

        [HttpDelete("DeleteImageById")]
        //[Authorize]
        public IActionResult DeleteImage(Guid id)
        {
            var imageEntity = _dbContext.ImagesStorage.FirstOrDefault(i => i.Id == id);
            if (imageEntity == null)
                return NotFound();

            string basee = Directory.GetCurrentDirectory();
            string folder = Path.Combine(basee, "Uploads");
            string filePath = Path.Combine(folder, imageEntity.ImageReference);

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            System.IO.File.Delete(filePath);
            _dbContext.ImagesStorage.Remove(imageEntity);
            _dbContext.SaveChanges();

            string message = "Image deleted successfully.";

            return Ok(new { message });
        }


    }
}
