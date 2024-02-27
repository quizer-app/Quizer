using ErrorOr;
using MediatR;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Commands.UpdateQuiz;

public record UpdateQuizCommand(
    Guid UserId,
    Guid ImageId,
    Guid QuizId,
    string Name,
    string Description) : IRequest<ErrorOr<QuizId>>;
