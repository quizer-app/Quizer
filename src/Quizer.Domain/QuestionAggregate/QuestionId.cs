using Quizer.Domain.Common.Models;

namespace Quizer.Domain.QuestionAggregate;

public sealed class QuestionId : AggregateRootId<Guid>
{

    private QuestionId(Guid value) : base(value)
    {
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
