using Quizer.Domain.Common.Models;

namespace Quizer.Domain.RefreshTokenAggregate;

public sealed class RefreshToken : AggregateRoot<TokenId, string>
{
    public bool IsValid { get; private set; }

    private RefreshToken(
        Guid userId,
        TokenId id,
        bool isValid
        ) : base(id, userId)
    {
        Id = id;
        IsValid = isValid;
    }

    public static RefreshToken Create(
        Guid userId,
        string token)
    {
        var refreshToken = new RefreshToken(
            userId,
            TokenId.Create(token),
            true);

        return refreshToken;
    }

    public void Invalidate(Guid userId)
    {
        this.Update(userId);
        IsValid = false;
    }

#pragma warning disable CS8618
    private RefreshToken() { }
#pragma warning restore CS8618
}
