using ErrorOr;
using Quizer.Application.Common.Cqrs;
using Quizer.Application.Quizes.Common;

namespace Quizer.Application.Quizes.Queries.GetQuizByName;

public record GetQuizByNameQuery(
    string UserName,
    string QuizSlug) : IQuery<ErrorOr<DetailedQuizResult>>;
