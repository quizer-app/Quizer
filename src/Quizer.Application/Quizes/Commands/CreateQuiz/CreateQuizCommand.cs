using ErrorOr;
using Quizer.Application.Common.Cqrs;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Commands.CreateQuiz;

public record CreateQuizCommand(
    string Name,
    string Description,
    Guid UserId,
    Guid ImageId,
    int? TimeLimitInSeconds,
    bool AutoSubmitWhenTimeExpires,
    int? MaxAttemptsPerUser) : ICommand<ErrorOr<QuizId>>;
