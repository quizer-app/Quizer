using ErrorOr;
using Quizer.Application.Services.Image.Response;

namespace Quizer.Application.Services.Image;

public interface IImageService
{
    Task<ErrorOr<DirectUploadResponse>> DirectUpload(bool requireSignedURLs = false);
}