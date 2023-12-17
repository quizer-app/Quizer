using Quizer.Domain.Entities;

namespace Quizer.Application.Services.Authentication.Common
{
    public record AuthenticationResult(
        User User,
        string Token
    );
}
