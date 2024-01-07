using ErrorOr;
using MediatR;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Application.Quizes.Common;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Queries.GetQuizByName;

public class GetQuizByNameQueryHandler : IRequestHandler<GetQuizByNameQuery, ErrorOr<DetailedQuizResult>>
{
    private readonly IQuizRepository _quizRepository;
    private readonly IQuestionRepository _questionRepository;

    public GetQuizByNameQueryHandler(IQuizRepository quizRepository, IQuestionRepository questionRepository)
    {
        _quizRepository = quizRepository;
        _questionRepository = questionRepository;
    }

    public async Task<ErrorOr<DetailedQuizResult>> Handle(GetQuizByNameQuery request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.Get(request.UserName, request.QuizSlug);
        if (quiz is null) return Errors.Quiz.NotFound;

        var quizId = (QuizId)quiz.Id;
        var questions = await _questionRepository.GetAllQuestions(quizId);

        return new DetailedQuizResult(quiz, questions);
    }
}
