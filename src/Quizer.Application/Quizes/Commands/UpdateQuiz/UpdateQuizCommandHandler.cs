using ErrorOr;
using MediatR;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Application.Services.Slugify;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Commands.UpdateQuiz;

public class UpdateQuizCommandHandler : IRequestHandler<UpdateQuizCommand, ErrorOr<QuizId>>
{
    private readonly IQuizRepository _quizRepository;
    private readonly ISlugifyService _slugifyService;

    public UpdateQuizCommandHandler(IQuizRepository quizRepository, ISlugifyService slugifyService)
    {
        _quizRepository = quizRepository;
        _slugifyService = slugifyService;
    }

    public async Task<ErrorOr<QuizId>> Handle(UpdateQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.Get(QuizId.Create(request.QuizId));

        if (quiz is null)
            return Errors.Quiz.NotFound;

        var id = (QuizId)quiz.Id;

        string slug = _slugifyService.GenerateSlug(request.Name);

        var result = quiz.Update(request.Name, slug, request.Description);
        if (result.IsError) return result.Errors;
        
        return id;
    }
}
