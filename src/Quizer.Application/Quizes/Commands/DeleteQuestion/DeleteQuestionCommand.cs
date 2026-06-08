using ErrorOr;
using Quizer.Application.Common.Cqrs;
using Quizer.Domain.QuestionAggregate;

namespace Quizer.Application.Quizes.Commands.DeleteQuestion;

public record DeleteQuestionCommand(
    Guid UserId,
    Guid QuestionId
    ) : ICommand<ErrorOr<QuestionId>>;
