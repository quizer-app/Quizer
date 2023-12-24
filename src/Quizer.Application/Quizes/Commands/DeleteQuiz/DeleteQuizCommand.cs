using ErrorOr;
using MediatR;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Commands.DeleteQuiz
{
    public record DeleteQuizCommand(
        QuizId QuizId
        ) : IRequest<ErrorOr<bool>>;

}
