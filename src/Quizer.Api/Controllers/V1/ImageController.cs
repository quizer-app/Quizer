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
    [HttpPost("")]
    public async Task<IActionResult> UploadImage()
    {
        var result = await _imageService.DirectUpload();

        return result.Match(
            Ok,
            Problem);
    }
}
