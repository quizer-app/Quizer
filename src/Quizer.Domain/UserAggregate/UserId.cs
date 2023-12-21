using Quizer.Domain.Common.Models;

namespace Quizer.Domain.UserAggregate
{
    public sealed class UserId : AggregateRootId<Guid>, IEquatable<UserId>
    {
        public override Guid Value { get; protected set; }

        private UserId(Guid value)
        {
            Value = value;
        }

        public static UserId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        public static UserId Create(Guid value)
        {
            return new(value);
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public bool Equals(UserId? other)
        {
            return base.Equals(other?.Value);
        }
    }
}
