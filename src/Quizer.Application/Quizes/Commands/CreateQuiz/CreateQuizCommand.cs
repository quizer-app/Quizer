using ErrorOr;
using MediatR;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Commands.CreateQuiz;

public record CreateQuizCommand(
    string Name,
    string Description,
    Guid UserId,
    Guid ImageId
    ) : IRequest<ErrorOr<QuizId>>;