using Quizer.Domain.UserAggregate;

namespace Quizer.Application.Authentication.Commands
{
    public record RegisterResult (
        User User
        );
}
