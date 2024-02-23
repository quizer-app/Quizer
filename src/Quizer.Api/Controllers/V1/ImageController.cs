using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizer.Application.Services.Image;

namespace Quizer.Api.Controllers.V1;

[Route("api/v{version:apiVersion}/[controller]")]

public class ImageController : ApiController
{
    private readonly IImageService _imageService;

    public ImageController(IImageService imageService)
    {
        _imageService = imageService;
    }

    [AllowAnonymous]
    [HttpPost("{imageType}/{id:guid}")]
    public async Task<IActionResult> UploadImage(
        [FromRoute] string imageType,
        [FromRoute] Guid id,
        [FromForm(Name = "Data")] IFormFile file)
    {
        var result = await _imageService.UploadImage(file, imageType);

        return result.Match(
            Ok,
            Problem);
    }
}
