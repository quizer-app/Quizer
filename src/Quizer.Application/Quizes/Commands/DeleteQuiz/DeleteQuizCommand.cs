using ErrorOr;
using Quizer.Application.Common.Cqrs;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Commands.DeleteQuiz;

public record DeleteQuizCommand(
    Guid UserId,
    Guid QuizId
    ) : ICommand<ErrorOr<QuizId>>;
