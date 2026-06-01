using Quizer.Domain.Common.Models;
using System.Security.Cryptography;
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

    public static TokenId FromToken(string token)
    {
        byte[] hash = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(token));
        return new(Convert.ToHexString(hash));
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
