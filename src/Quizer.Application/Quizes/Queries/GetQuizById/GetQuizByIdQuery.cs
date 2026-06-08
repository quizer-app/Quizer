using ErrorOr;
using Quizer.Application.Common.Cqrs;
using Quizer.Application.Quizes.Common;

namespace Quizer.Application.Quizes.Queries.GetQuizById;

public record GetQuizByIdQuery(
    Guid QuizId) : IQuery<ErrorOr<DetailedQuizResult>>;
