using ErrorOr;
using MediatR;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Queries.GetQuiz
{
    public record GetQuizQuery(
        string? QuizId,
        string? Name) : IRequest<ErrorOr<Quiz>>;
}
