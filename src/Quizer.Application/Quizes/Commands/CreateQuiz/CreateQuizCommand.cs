using ErrorOr;
using MediatR;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Commands.CreateQuiz;

public record CreateQuizCommand(
    string Name,
    string Description,
    Guid UserId,
    List<CreateQuizQuestionCommand> Questions
    ) : IRequest<ErrorOr<QuizId>>;

public record CreateQuizQuestionCommand(
    string QuestionText,
    string Answer);
