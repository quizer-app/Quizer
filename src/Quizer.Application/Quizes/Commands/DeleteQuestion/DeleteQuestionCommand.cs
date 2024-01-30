using ErrorOr;
using MediatR;
using Quizer.Domain.QuestionAggregate;

namespace Quizer.Application.Quizes.Commands.DeleteQuestion;

public record DeleteQuestionCommand(
    Guid UserId,
    Guid QuestionId
    ) : IRequest<ErrorOr<QuestionId>>;
