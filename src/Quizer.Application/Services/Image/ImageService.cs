﻿using ImageMagick;
using Microsoft.Extensions.Logging;

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
        string imageDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imageType);
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
}
