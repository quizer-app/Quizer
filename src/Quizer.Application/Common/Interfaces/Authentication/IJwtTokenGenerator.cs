using Quizer.Domain.UserAggregate;

namespace Quizer.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken(User user);
    string GenerateAccessToken(string refreshToken);
    Task<bool> ValidateRefreshToken(string refreshToken);
}
