using ErrorOr;
using MediatR;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Commands.UpdateQuiz;

public record UpdateQuizCommand(
    Guid UserId,
    Guid ImageId,
    Guid QuizId,
    string Name,
    string Description,
    int? TimeLimitInSeconds,
    bool AutoSubmitWhenTimeExpires,
    int? MaxAttemptsPerUser) : IRequest<ErrorOr<QuizId>>;
