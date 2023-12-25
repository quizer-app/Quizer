using ErrorOr;
using MediatR;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Commands.DeleteQuiz;

public record DeleteQuizCommand(
    Guid QuizId
    ) : IRequest<ErrorOr<QuizId>>;
