using Quizer.Domain.UserAggregate;

namespace Quizer.Application.Authentication.Queries
{
    public record LoginResult(
        User User,
        string Token
    );
}
