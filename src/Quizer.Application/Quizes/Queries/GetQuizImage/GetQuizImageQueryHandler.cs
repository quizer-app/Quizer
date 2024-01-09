using ErrorOr;
using MediatR;
using Quizer.Application.Services.Image;

namespace Quizer.Application.Quizes.Queries.GetQuizImage;

public class GetQuizImageQueryHandler : IRequestHandler<GetQuizImageQuery, ErrorOr<QuizImageResponse>>
{
    private readonly IImageService _imageService;

    public GetQuizImageQueryHandler(IImageService imageService)
    {
        _imageService = imageService;
    }

    public async Task<ErrorOr<QuizImageResponse>> Handle(GetQuizImageQuery request, CancellationToken cancellationToken)
    {
        string imagePath = _imageService.GetImagePath("quiz", request.QuizId);
        var imageData = File.ReadAllBytes(imagePath);

        return new QuizImageResponse(imageData, "image/webp");
    }
}
