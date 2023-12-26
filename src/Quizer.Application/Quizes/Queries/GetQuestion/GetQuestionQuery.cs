using ErrorOr;
using MediatR;
using Quizer.Domain.QuestionAggregate;

namespace Quizer.Application.Quizes.Queries.GetQuestion;

public record GetQuestionQuery(Guid QuestionId): IRequest<ErrorOr<Question>>;
