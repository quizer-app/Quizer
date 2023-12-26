using Quizer.Domain.Common.Models;

namespace Quizer.Domain.QuestionAggregate.Events;

public record QuestionDeleted(Question question) : IDomainEvent;
