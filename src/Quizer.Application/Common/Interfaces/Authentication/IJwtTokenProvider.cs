using Quizer.Domain.UserAggregate;

namespace Quizer.Application.Common.Interfaces.Authentication;

public interface IJwtTokenProvider
{
    Task<string> GenerateAccessToken(User user);
    Task<string> GenerateRefreshToken(User user);
    string GenerateAccessToken(string refreshToken);
    Task<bool> ValidateRefreshToken(string refreshToken);
}
