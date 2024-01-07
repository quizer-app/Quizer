using ErrorOr;
using MediatR;
using Quizer.Application.Quizes.Common;

namespace Quizer.Application.Quizes.Queries.GetQuizByName;

public record GetQuizByNameQuery(
    string UserName,
    string QuizSlug) : IRequest<ErrorOr<DetailedQuizResult>>;
