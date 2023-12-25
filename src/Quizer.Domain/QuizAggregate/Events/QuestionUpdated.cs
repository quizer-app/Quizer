using Quizer.Domain.Common.Models;
using Quizer.Domain.QuizAggregate.Entities;

namespace Quizer.Domain.QuizAggregate.Events;

public record QuestionUpdated(Question question) : IDomainEvent;
