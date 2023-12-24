using Quizer.Domain.Common.Models;

namespace Quizer.Domain.QuizAggregate.Events
{
    public record QuizCreated(Quiz quiz) : IDomainEvent;
}
