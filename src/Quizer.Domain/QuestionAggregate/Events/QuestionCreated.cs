using Quizer.Domain.Common.Models;
using Quizer.Domain.QuizAggregate;

namespace Quizer.Domain.QuestionAggregate.Events;

public record QuestionCreated(Question Question) : IDomainEvent;
