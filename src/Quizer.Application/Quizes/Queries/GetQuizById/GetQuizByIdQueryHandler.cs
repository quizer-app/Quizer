using ErrorOr;
using MediatR;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Queries.GetQuizById;

public class GetQuizByIdQueryHandler : IRequestHandler<GetQuizByIdQuery, ErrorOr<Quiz>>
{
    private readonly IQuizRepository _quizRepository;

    public GetQuizByIdQueryHandler(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<ErrorOr<Quiz>> Handle(GetQuizByIdQuery request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.Get(QuizId.Create(request.QuizId));
        if (quiz is null) return Errors.Quiz.NotFound;
        return quiz;
    }
}
