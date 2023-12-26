using Quizer.Domain.Common.Models;

namespace Quizer.Domain.QuestionAggregate.ValueObjects;

public sealed class AnswerId : EntityId<Guid>
{
    public AnswerId(Guid value) : base(value)
    {
    }

    public static AnswerId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public static AnswerId Create(Guid value)
    {
        return new(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
