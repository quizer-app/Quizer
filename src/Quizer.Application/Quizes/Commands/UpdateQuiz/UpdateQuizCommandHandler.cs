using ErrorOr;
using MediatR;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Application.Services.Image;
using Quizer.Application.Services.Slugify;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.Common.ValueObjects;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Commands.UpdateQuiz;

public class UpdateQuizCommandHandler : IRequestHandler<UpdateQuizCommand, ErrorOr<QuizId>>
{
    private readonly IQuizRepository _quizRepository;
    private readonly ISlugifyService _slugifyService;
    private readonly IImageService _imageService;

    public UpdateQuizCommandHandler(IQuizRepository quizRepository, ISlugifyService slugifyService, IImageService imageService)
    {
        _quizRepository = quizRepository;
        _slugifyService = slugifyService;
        _imageService = imageService;
    }

    public async Task<ErrorOr<QuizId>> Handle(UpdateQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.Get(QuizId.Create(request.QuizId));

        if (quiz is null)
            return Errors.Quiz.NotFound;

        var id = (QuizId)quiz.Id;

        string slug = _slugifyService.GenerateSlug(request.Name);

        if(quiz.Image.ImageId != request.ImageId)
        {
            bool deleted = await _imageService.DeleteImage(quiz.Image.ImageId);
            if (!deleted)
                return Errors.Image.CannotDelete;

            bool uploaded = await _imageService.IsSuccessfulyUploaded(request.ImageId);
            if (!uploaded)
                return Errors.Image.CannotUpload;
        }

        var result = quiz.Update(
            request.UserId,
            Image.CreateNew(request.ImageId, _imageService.GenerateImageUrl(request.ImageId, "quiz")),
            request.Name,
            slug,
            request.Description);

        if (result.IsError) return result.Errors;
        
        return id;
    }
}
