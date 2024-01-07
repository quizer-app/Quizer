using ErrorOr;
using MediatR;
using Quizer.Application.Quizes.Common;

namespace Quizer.Application.Quizes.Queries.GetQuizById;

public record GetQuizByIdQuery(
    Guid QuizId) : IRequest<ErrorOr<DetailedQuizResult>>;
