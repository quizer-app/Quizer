using Quizer.Domain.Entities;

namespace Quizer.Application.Services.Authentication
{
    public record AuthenticationResult (
        User User,
        string Token
    );
}
