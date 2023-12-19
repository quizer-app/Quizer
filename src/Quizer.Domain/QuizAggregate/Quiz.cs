using Quizer.Domain.Common.Models;
using Quizer.Domain.QuizAggregate.Entities;
using Quizer.Domain.QuizAggregate.ValueObjects;

namespace Quizer.Domain.QuizAggregate
{
    public sealed class Quiz : AggregateRoot<QuizId>
    {
        private readonly List<Question> _questions = new();

        public string Name { get; }
        public string Description { get; }
        public float AverageRating { get; }
        public IReadOnlyList<Question> Questions => _questions.AsReadOnly();

        public DateTime CreatedAt { get; }
        public DateTime UpdatedAt { get; }

        private Quiz(
            QuizId id,
            string name,
            string description,
            float averageRating,
            DateTime createdAt,
            DateTime updatedAt) : base(id)
        {
            Name = name;
            Description = description;
            AverageRating = averageRating;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public static Quiz Create(
            string name,
            string description,
            float averageRating)
        {
            return new(
                QuizId.CreateUnique(),
                name,
                description,
                averageRating,
                DateTime.UtcNow,
                DateTime.UtcNow);
        }
    }
}
