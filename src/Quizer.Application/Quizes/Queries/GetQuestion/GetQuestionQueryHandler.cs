using ErrorOr;
using MediatR;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.QuizAggregate.Entities;
using Quizer.Domain.QuizAggregate.ValueObjects;

namespace Quizer.Application.Quizes.Queries.GetQuestion;

public class GetQuestionQueryHandler : IRequestHandler<GetQuestionQuery, ErrorOr<Question>>
{
    private readonly IQuizRepository _quizRepository;

    public GetQuestionQueryHandler(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<ErrorOr<Question>> Handle(GetQuestionQuery request, CancellationToken cancellationToken)
    {
        var question = await _quizRepository.GetQuestion(QuestionId.Create(request.QuestionId));
        if (question is null) return Errors.Question.NotFound;
        return question;
    }
}
