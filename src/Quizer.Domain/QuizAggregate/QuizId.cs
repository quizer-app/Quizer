using Quizer.Domain.Common.Models;

namespace Quizer.Domain.QuizAggregate;

public sealed class QuizId : AggregateRootId<Guid>
{
    private QuizId(Guid value) : base(value)
    {
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
