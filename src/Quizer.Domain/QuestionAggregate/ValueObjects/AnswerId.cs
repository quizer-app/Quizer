using Quizer.Domain.Common.Models;

namespace Quizer.Domain.QuestionAggregate.ValueObjects;

public sealed class AnswerId : ValueObject
{
    public Guid Value { get; private set; }

    private AnswerId(Guid value)
    {
        Value = value;
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
