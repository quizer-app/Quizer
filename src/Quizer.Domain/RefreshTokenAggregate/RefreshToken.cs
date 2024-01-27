using Quizer.Domain.Common.Models;

namespace Quizer.Domain.RefreshTokenAggregate;

public sealed class RefreshToken : AggregateRoot<TokenId, string>
{
    public bool IsValid { get; private set; }

    private RefreshToken(
        TokenId id,
        bool isValid
        ) : base(id)
    {
        Id = id;
        IsValid = isValid;
    }

    public static RefreshToken Create(
        string token)
    {
        var refreshToken = new RefreshToken(
            TokenId.Create(token),
            true);

        return refreshToken;
    }

    public void Invalidate()
    {
        IsValid = false;
    }

#pragma warning disable CS8618
    private RefreshToken() { }
#pragma warning restore CS8618
}
