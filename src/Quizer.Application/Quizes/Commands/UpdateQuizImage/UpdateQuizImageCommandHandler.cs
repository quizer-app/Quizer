using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Quizer.Application.Services.Image;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Commands.UpdateQuizImage;

public class UpdateQuizImageCommandHandler : IRequestHandler<UpdateQuizImageCommand, ErrorOr<QuizId>>
{
    private readonly IImageService _imageService;

    public UpdateQuizImageCommandHandler(IImageService imageService)
    {
        _imageService = imageService;
    }

    public async Task<ErrorOr<QuizId>> Handle(UpdateQuizImageCommand request, CancellationToken cancellationToken)
    {
        string[] correctExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

        if (!IsCorrectExtension(request.File.FileName, correctExtensions))
            return Errors.Image.WrongFormat(correctExtensions);

        var id = QuizId.Create(request.QuizId);

        string tempFilePath = await SaveTempFile(request.File);

        string imageDir = _imageService.GetImageDir("quiz");

        var imageProcessResult = ProcessImage(tempFilePath, imageDir, id.Value);
        if (imageProcessResult.IsError)
            return imageProcessResult.Errors;

        return id;
    }

    private ErrorOr<string> ProcessImage(string tempFilePath, string imageDir, Guid id)
    {
        string? imagePath = _imageService.FormatAndMove(tempFilePath, imageDir, id);
        if (imagePath is null)
            return Errors.Image.CannotUpload;

        bool resized = _imageService.Resize(imagePath, 512, 288);
        if (!resized)
            return Errors.Image.CannotUpload;

        return imagePath;
    }

    private bool IsCorrectExtension(string fileName, string[] correctExtensions)
    {
        string extension = Path.GetExtension(fileName);

        return correctExtensions.Contains(extension);
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
}
