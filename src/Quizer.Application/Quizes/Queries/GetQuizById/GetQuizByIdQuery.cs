using ErrorOr;
using MediatR;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Queries.GetQuizById;

public record GetQuizByIdQuery(
    Guid QuizId) : IRequest<ErrorOr<Quiz>>;
