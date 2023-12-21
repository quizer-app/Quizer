using Quizer.Domain.Common.Models;
using Quizer.Domain.Common.ValueObjects;
using Quizer.Domain.QuizAggregate.Entities;
using Quizer.Domain.UserAggregate;

namespace Quizer.Domain.QuizAggregate
{
    public sealed class Quiz : AggregateRoot<QuizId, Guid>
    {
        private readonly List<Question> _questions = new();

        public UserId UserId { get; private set; }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public AverageRating AverageRating { get; private set; }
        public IReadOnlyList<Question> Questions => _questions.AsReadOnly();

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private Quiz(
            QuizId id,
            UserId userId,
            string name,
            string description,
            AverageRating averageRating,
            List<Question> questions,
            DateTime createdAt,
            DateTime updatedAt) : base(id)
        {
            Name = name;
            UserId = userId;
            Description = description;
            AverageRating = averageRating;
            _questions = questions;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public static Quiz Create(
            string name,
            string description,
            UserId userId,
            AverageRating averageRating,
            List<Question> questions)
        {
            return new(
                QuizId.CreateUnique(),
                userId,
                name,
                description,
                averageRating,
                questions,
                DateTime.UtcNow,
                DateTime.UtcNow);
        }

#pragma warning disable CS8618
        private Quiz()
        {
        }
#pragma warning restore CS8618
    }
}
