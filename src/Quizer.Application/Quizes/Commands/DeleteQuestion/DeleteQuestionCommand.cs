using ErrorOr;
using MediatR;
using Quizer.Domain.QuizAggregate.ValueObjects;

namespace Quizer.Application.Quizes.Commands.DeleteQuestion;

public record DeleteQuestionCommand(
    Guid QuestionId
    ) : IRequest<ErrorOr<QuestionId>>;
