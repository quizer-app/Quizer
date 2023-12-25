using ErrorOr;
using MediatR;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Commands.CreateQuiz;

public record CreateQuizCommand(
    string Name,
    string Description,
    Guid UserId,
    List<QuestionCommand> Questions
    ) : IRequest<ErrorOr<QuizId>>;

public record QuestionCommand(
    string QuestionText,
    string Answer);
