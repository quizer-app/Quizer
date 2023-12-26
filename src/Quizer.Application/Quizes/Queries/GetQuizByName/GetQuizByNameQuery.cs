using ErrorOr;
using MediatR;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Application.Quizes.Queries.GetQuizByName;

public record GetQuizByNameQuery(
    string UserName,
    string QuizName) : IRequest<ErrorOr<Quiz>>;
