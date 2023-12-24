using ErrorOr;
using MediatR;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Queries.GetQuiz
{
    public record GetQuizQuery(
        Guid? QuizId,
        string? Name) : IRequest<ErrorOr<Quiz>>;
}
