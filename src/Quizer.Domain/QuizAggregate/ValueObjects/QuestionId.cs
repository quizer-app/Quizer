using Quizer.Domain.Common.Models;

namespace Quizer.Domain.QuizAggregate.ValueObjects
{
    public sealed class QuestionId : ValueObject
    {
        public Guid Value { get; private set; }

        private QuestionId(Guid value)
        {
            Value = value;
        }

        public static QuestionId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        public static QuestionId Create(Guid value)
        {
            return new(value);
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
