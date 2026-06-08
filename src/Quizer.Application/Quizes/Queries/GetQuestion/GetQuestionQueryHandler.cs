using ErrorOr;
using Quizer.Application.Common.Cqrs;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.QuestionAggregate;

namespace Quizer.Application.Quizes.Queries.GetQuestion;

public class GetQuestionQueryHandler : IQueryHandler<GetQuestionQuery, ErrorOr<Question>>
{
    private readonly IQuestionRepository _questionRepository;

    public GetQuestionQueryHandler(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<ErrorOr<Question>> Handle(GetQuestionQuery request, CancellationToken cancellationToken)
    {
        var question = await _questionRepository.Get(QuestionId.Create(request.QuestionId));
        if (question is null) return Errors.Question.NotFound;
        return question;
    }
}
