using ErrorOr;
using MediatR;
using Quizer.Application.Quizes.Common;
using Quizer.Domain.QuestionAggregate;

namespace Quizer.Application.Quizes.Commands.UpdateQuestion;

public record UpdateQuestionCommand(
    Guid QuestionId,
    string QuestionText,
    List<CreateQuestionAnswer> Answers
    ) : IRequest<ErrorOr<QuestionId>>;
