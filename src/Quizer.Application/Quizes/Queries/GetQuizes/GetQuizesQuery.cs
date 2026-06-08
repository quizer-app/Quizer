using ErrorOr;
using Quizer.Application.Common.Cqrs;
using Quizer.Application.Utils;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Queries.GetQuizes;

public record GetQuizesQuery(
    string? SearchTerm,
    string? SortColumn,
    string? SortOrder,
    int PageNumber,
    int PageSize,
    string? UserName) : IQuery<ErrorOr<PagedList<Quiz>>>;
