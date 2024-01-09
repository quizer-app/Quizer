using ErrorOr;
using MediatR;
using Quizer.Application.Utils;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Queries.GetQuizes;

public record GetQuizesQuery(
    string? SearchTerm,
    string? SortColumn,
    string? SortOrder,
    int PageNumber,
    int PageSize,
    string? UserName) : IRequest<ErrorOr<PagedList<Quiz>>>;
