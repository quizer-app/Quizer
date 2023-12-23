using ErrorOr;
using MediatR;
using Quizer.Application.Authentication.Common;

namespace Quizer.Application.Authentication.Commands
{
    public record RegisterCommand(
        string Username,
        string Email,
        string Password
        ) : IRequest<ErrorOr<AuthenticationResult>>;
}
