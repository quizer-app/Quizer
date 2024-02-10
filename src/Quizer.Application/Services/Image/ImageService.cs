using ErrorOr;
using ImageMagick;
using Microsoft.Extensions.Logging;
using Quizer.Domain.Common.Errors;
using Microsoft.AspNetCore.Http;

namespace Quizer.Application.Services.Image;

public class ImageService : IImageService
{
    private readonly ILogger<ImageService> _logger;
    private readonly ImageOptimizer _optimizer;

    public ImageService(ILogger<ImageService> logger)
    {
        _optimizer = new ImageOptimizer();
        _logger = logger;
    }

    public async Task<ErrorOr<string>> UploadImage(IFormFile file, string imageType, Guid id)
    {
        string[] correctExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        string[] correctImageTypes = { "quiz" };

        if(!correctImageTypes.Contains(imageType))
            return Errors.Image.WrongType(correctImageTypes);

        if (!IsCorrectExtension(file.FileName, correctExtensions))
            return Errors.Image.WrongFormat(correctExtensions);

        string tempFilePath = await SaveTempFile(file);

        string imageDir = GetImageDir(imageType);

        var imageProcessResult = ProcessImage(tempFilePath, imageDir, id);
        if (imageProcessResult.IsError)
            return imageProcessResult.Errors;

        return $"/images/{imageType}/{id}";
    }

    public string? FormatAndMove(string filePathIn, string dirPathOut, Guid id)
    {
        try
        {
            string fileName = $"{id}.webp";
            string filePathOut = Path.Combine(dirPathOut, fileName);

            using var image = new MagickImage(filePathIn);
            image.Format = MagickFormat.WebP;

            image.Write(filePathOut);

            return filePathOut;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occured during image format: {@Exception}", ex);
            return null;
        }
    }

    public bool Resize(string filePath, int width, int height)
    {
        try
        {
            using var image = new MagickImage(filePath);
            image.Resize(width, height);
            image.Write(filePath);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occured during image resize: {@Exception}", ex);
            return false;
        }
        return true;
    }

    public bool Optimize(string filePath)
    {
        try
        {
            var fileInfo = new FileInfo(filePath);
            _optimizer.LosslessCompress(fileInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occured during image optimize: {@Exception}", ex);
            return false;
        }
        return true;
    }

    public string GetImageDir(string imageType)
    {
        string imageDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files", "images", imageType);
        if (!Directory.Exists(imageDir))
            Directory.CreateDirectory(imageDir);

        return imageDir;
    }

    public string GetImagePath(string imageType, Guid id)
    {
        string imageDir = GetImageDir(imageType);
        string filePath = Path.Combine(imageDir, $"{id}.webp");
        if (!File.Exists(filePath))
        {
            filePath = Path.Combine(imageDir, $"default.webp");
        }

        return filePath;
    }

    private async Task<string> SaveTempFile(IFormFile file)
    {
        string tempFilePath = Path.GetTempFileName();
        using (var stream = new FileStream(tempFilePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return tempFilePath;
    }

    private bool IsCorrectExtension(string fileName, string[] correctExtensions)
    {
        string extension = Path.GetExtension(fileName);

        return correctExtensions.Contains(extension);
    }

    private ErrorOr<string> ProcessImage(string tempFilePath, string imageDir, Guid id)
    {
        string? imagePath = FormatAndMove(tempFilePath, imageDir, id);
        if (imagePath is null)
            return Errors.Image.CannotUpload;

        bool resized = Resize(imagePath, 512, 288);
        if (!resized)
            return Errors.Image.CannotUpload;

        return imagePath;
    }
}
