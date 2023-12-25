using ErrorOr;
using MediatR;
using Quizer.Domain.QuizAggregate.ValueObjects;

namespace Quizer.Application.Quizes.Commands.CreateQuestion;

public record CreateQuestionCommand(
    Guid QuizId,
    string QuestionText,
    string Answer
    ) : IRequest<ErrorOr<QuestionId>>;
