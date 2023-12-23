using ErrorOr;
using MediatR;
using Quizer.Domain.UserAggregate;

namespace Quizer.Application.Authentication.Commands
{
    public record RegisterCommand(
        string Username,
        string Email,
        string Password
        ) : IRequest<ErrorOr<User>>;
}
