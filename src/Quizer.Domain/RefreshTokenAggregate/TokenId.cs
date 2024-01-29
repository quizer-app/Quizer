using Quizer.Domain.Common.Models;

namespace Quizer.Domain.RefreshTokenAggregate;

public sealed class TokenId : AggregateRootId<string>
{
    private TokenId(string value) : base(value)
    {
    }

    public static TokenId Create(string value)
    {
        return new(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
