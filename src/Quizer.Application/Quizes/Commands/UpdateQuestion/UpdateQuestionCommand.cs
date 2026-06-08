using ErrorOr;
using Quizer.Application.Common.Cqrs;
using Quizer.Application.Quizes.Common;
using Quizer.Domain.QuestionAggregate;

namespace Quizer.Application.Quizes.Commands.UpdateQuestion;

public record UpdateQuestionCommand(
    Guid UserId,
    Guid QuestionId,
    string QuestionText,
    List<CreateQuestionAnswer> Answers
    ) : ICommand<ErrorOr<QuestionId>>;
