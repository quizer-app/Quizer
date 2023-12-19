using Quizer.Domain.Common.Models;

namespace Quizer.Domain.QuizAggregate.ValueObjects
{
    public sealed class QuizId : ValueObject
    {
        public Guid Value { get; }

        private QuizId(Guid value)
        {
            Value = value;
        }

        public static QuizId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        public static QuizId Create(Guid value)
        {
            return new(value);
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
