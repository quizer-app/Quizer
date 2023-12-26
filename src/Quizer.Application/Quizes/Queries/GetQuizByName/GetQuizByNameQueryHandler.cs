using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Queries.GetQuizByName;

public class GetQuizByNameQueryHandler : IRequestHandler<GetQuizByNameQuery, ErrorOr<Quiz>>
{
    private readonly IQuizRepository _quizRepository;

    public GetQuizByNameQueryHandler(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<ErrorOr<Quiz>> Handle(GetQuizByNameQuery request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.Get(request.UserName, request.QuizName);
        if (quiz is null) return Errors.Quiz.NotFound;

        return quiz;
    }
}
