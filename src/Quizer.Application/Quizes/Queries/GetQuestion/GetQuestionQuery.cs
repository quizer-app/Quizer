using ErrorOr;
using Quizer.Application.Common.Cqrs;
using Quizer.Domain.QuestionAggregate;

namespace Quizer.Application.Quizes.Queries.GetQuestion;

public record GetQuestionQuery(Guid QuestionId): IQuery<ErrorOr<Question>>;
