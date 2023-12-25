using Quizer.Domain.Common.Models;

namespace Quizer.Domain.QuizAggregate.Events;

public record QuizUpdated(Quiz quiz) : IDomainEvent;
