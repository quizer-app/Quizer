using Quizer.Domain.Entities;

namespace Quizer.Application.Authentication.Common
{
    public record AuthenticationResult(
        User User,
        string Token
    );
}
