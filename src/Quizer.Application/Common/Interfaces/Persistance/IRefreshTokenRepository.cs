using Quizer.Domain.RefreshTokenAggregate;

namespace Quizer.Application.Common.Interfaces.Persistance;

public interface IRefreshTokenRepository
{
    Task Add(RefreshToken refreshToken);
    void Delete(RefreshToken refreshToken);
    Task<RefreshToken?> Get(TokenId id);
}
