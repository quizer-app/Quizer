
using ErrorOr;
using Microsoft.AspNetCore.Http;

namespace Quizer.Application.Services.Image
{
    public interface IImageService
    {
        Task<ErrorOr<string>> UploadImage(IFormFile file, string imageType);
        string? FormatAndMove(string filePathIn, string dirPathOut, Guid id);
        bool Optimize(string filePath);
        bool Resize(string filePath, int width, int height);
        string GetImageDir(string imageType);
        string GetImagePath(string imageType, Guid id);
    }
}