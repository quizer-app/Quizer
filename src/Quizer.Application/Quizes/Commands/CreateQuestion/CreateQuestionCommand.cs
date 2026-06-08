using ErrorOr;
using Quizer.Application.Common.Cqrs;
using Quizer.Application.Quizes.Common;
using Quizer.Domain.QuestionAggregate;

namespace Quizer.Application.Quizes.Commands.CreateQuestion;

public record CreateQuestionCommand(
    Guid UserId,
    Guid QuizId,
    string QuestionText,
    List<CreateQuestionAnswer> Answers
    ) : ICommand<ErrorOr<QuestionId>>;
