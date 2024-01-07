using ErrorOr;
using MediatR;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Application.Quizes.Common;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Queries.GetQuizById;

public class GetQuizByIdQueryHandler : IRequestHandler<GetQuizByIdQuery, ErrorOr<DetailedQuizResult>>
{
    private readonly IQuizRepository _quizRepository;
    private readonly IQuestionRepository _questionRepository;

    public GetQuizByIdQueryHandler(IQuizRepository quizRepository, IQuestionRepository questionRepository)
    {
        _quizRepository = quizRepository;
        _questionRepository = questionRepository;
    }

    public async Task<ErrorOr<DetailedQuizResult>> Handle(GetQuizByIdQuery request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.Get(QuizId.Create(request.QuizId));
        if (quiz is null) return Errors.Quiz.NotFound;

        var quizId = (QuizId)quiz.Id;
        var questions = await _questionRepository.GetAllQuestions(quizId);

        return new DetailedQuizResult(quiz, questions);
    }
}
