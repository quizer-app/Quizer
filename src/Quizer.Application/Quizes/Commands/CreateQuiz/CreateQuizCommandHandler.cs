using ErrorOr;
using MediatR;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.Common.ValueObjects;
using Quizer.Domain.QuizAggregate;
using Quizer.Domain.QuizAggregate.Entities;

namespace Quizer.Application.Quizes.Commands.CreateQuiz;

public class CreateQuizCommandHandler : IRequestHandler<CreateQuizCommand, ErrorOr<QuizId>>
{
    private readonly IQuizRepository _quizRepository;

    public CreateQuizCommandHandler(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<ErrorOr<QuizId>> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
    {
        if ((await _quizRepository.Get(request.Name)) is not null)
            return Errors.Quiz.DuplicateName;

        var quiz = Quiz.Create(
            request.Name,
            request.Description,
            request.UserId,
            AverageRating.CreateNew(),
            request.Questions
                .ConvertAll(q => Question.Create(
                    q.QuestionText,
                    q.Answer)));

        await _quizRepository.Add(quiz);

        return (QuizId)quiz.Id;
    }
}
