using ErrorOr;
using MediatR;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Queries.GetQuiz;

public class GetQuizQueryHandler : IRequestHandler<GetQuizQuery, ErrorOr<Quiz>>
{
    private readonly IQuizRepository _quizRepository;

    public GetQuizQueryHandler(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<ErrorOr<Quiz>> Handle(GetQuizQuery request, CancellationToken cancellationToken)
    {
        if (request.Name is not null)
        {
            var quiz = await _quizRepository.Get(request.Name);
            if (quiz is null) return Errors.Quiz.NotFound;
            return quiz;
        }
        if (request.QuizId is not null)
        {
            var quiz = await _quizRepository.Get(QuizId.Create((Guid)request.QuizId));
            if (quiz is null) return Errors.Quiz.NotFound;
            return quiz;
        }

        return Errors.Quiz.NotFound;
    }
}
