using Quizer.Domain.Common.Models;

namespace Quizer.Domain.QuestionAggregate;

public sealed class QuestionId : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }

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
