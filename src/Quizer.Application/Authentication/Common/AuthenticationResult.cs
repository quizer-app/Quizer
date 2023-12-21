using Quizer.Domain.UserAggregate;

namespace Quizer.Application.Authentication.Common
{
    public record AuthenticationResult(
        User User,
        string Token
    );
}
