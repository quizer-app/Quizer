using Quizer.Domain.Common.Models;
using Quizer.Domain.Common.ValueObjects;
using Quizer.Domain.QuizAggregate.Entities;
using Quizer.Domain.QuizAggregate.Events;

namespace Quizer.Domain.QuizAggregate
{
    public sealed class Quiz : AggregateRoot<QuizId, Guid>
    {
        private readonly List<Question> _questions = new();

        public Guid UserId { get; private set; }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public AverageRating AverageRating { get; private set; }
        public IReadOnlyList<Question> Questions => _questions.AsReadOnly();

        private Quiz(
            QuizId id,
            Guid userId,
            string name,
            string description,
            AverageRating averageRating,
            List<Question> questions) : base(id)
        {
            Name = name;
            UserId = userId;
            Description = description;
            AverageRating = averageRating;
            _questions = questions;
        }

        public static Quiz Create(
            string name,
            string description,
            Guid userId,
            AverageRating averageRating,
            List<Question> questions)
        {
            var quiz = new Quiz(
                QuizId.CreateUnique(),
                userId,
                name,
                description,
                averageRating,
                questions);

            quiz.AddDomainEvent(new QuizCreated(quiz));

            return quiz;
        }

        public void Update(
            string name,
            string description)
        {
            base.Update();
            Name = name;
            Description = description;

            this.AddDomainEvent(new QuizUpdated(this));
        }

        public void Delete()
        {
            this.AddDomainEvent(new QuizDeleted(this));
        }

#pragma warning disable CS8618
        private Quiz()
        {
        }
#pragma warning restore CS8618
    }
}
