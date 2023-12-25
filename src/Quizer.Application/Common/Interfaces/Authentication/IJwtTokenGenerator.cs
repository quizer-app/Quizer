using Quizer.Domain.UserAggregate;

namespace Quizer.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
