using ErrorOr;
using MediatR;
using Quizer.Domain.QuestionAggregate;

namespace Quizer.Application.Quizes.Commands.UpdateQuestion;

public record UpdateQuestionCommand(
    Guid QuestionId,
    string QuestionText,
    string Answer) : IRequest<ErrorOr<QuestionId>>;
