using Quizer.Domain.Common.Models;
using Quizer.Domain.QuizAggregate.ValueObjects;

namespace Quizer.Domain.QuizAggregate.Entities
{
    public sealed class Question : Entity<QuestionId>
    {
        public string QuestionText { get; }
        public string Answer { get; }

        private Question(QuestionId id, string questionText, string answer) : base(id)
        {
            QuestionText = questionText;
            Answer = answer;
        }

        public static Question Create(string questionText, string answer)
        {
            return new(QuestionId.CreateUnique(), questionText, answer);
        }
    }
}
