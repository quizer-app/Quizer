using Quizer.Domain.Common.Models;

namespace Quizer.Domain.QuizAggregate.Events;

public record QuizDeleted(Quiz quiz) : IDomainEvent;
