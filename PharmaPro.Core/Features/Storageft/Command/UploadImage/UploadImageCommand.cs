using MediatR;
using Microsoft.AspNetCore.Http;
using PharmaPro.Core.Contract.Api;
using System.ComponentModel.DataAnnotations;

namespace PharmaPro.Core.Features.Storageft.Command.UploadImage
{
    public record UploadImageCommand([Required] IFormFile file) : IRequest<APIResponse<UploadImageCommandResponse>>;
}