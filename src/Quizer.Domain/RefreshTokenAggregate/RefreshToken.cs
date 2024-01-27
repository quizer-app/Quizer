using Quizer.Domain.Common.Models;

namespace Quizer.Domain.RefreshTokenAggregate;

public sealed class RefreshToken : AggregateRoot<TokenId, string>
{

}
