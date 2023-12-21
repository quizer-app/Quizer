using ErrorOr;
using MediatR;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Queries.GetQuizes
{
    public record GetQuizesQuery(
        string? UserId = null) : IRequest<ErrorOr<List<Quiz>>>;
}
