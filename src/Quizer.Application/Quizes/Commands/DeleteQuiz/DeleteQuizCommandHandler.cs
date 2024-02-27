using ErrorOr;
using MediatR;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Application.Services.Image;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Commands.DeleteQuiz;

public class DeleteQuizCommandHandler : IRequestHandler<DeleteQuizCommand, ErrorOr<QuizId>>
{
    private readonly IQuizRepository _quizRepository;
    private readonly IImageService _imageService;

    public DeleteQuizCommandHandler(IQuizRepository quizRepository, IImageService imageService)
    {
        _quizRepository = quizRepository;
        _imageService = imageService;
    }

    public async Task<ErrorOr<QuizId>> Handle(DeleteQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.Get(QuizId.Create(request.QuizId));

        if (quiz is null)
            return Errors.Quiz.NotFound;

        bool deleted = await _imageService.DeleteImage(quiz.Image.ImageId);
        if (!deleted)
            return Errors.Image.CannotDelete;

        var id = (QuizId)quiz.Id;
        quiz.Delete();
        _quizRepository.Delete(quiz);

        return id;
    }
}
