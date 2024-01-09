
namespace Quizer.Application.Services.Image
{
    public interface IImageService
    {
        string? FormatAndMove(string filePathIn, string dirPathOut, Guid id);
        bool Optimize(string filePath);
        bool Resize(string filePath, int width, int height);
        string GetImageDir(string imageType);
        string GetImagePath(string imageType, Guid id);
    }
}