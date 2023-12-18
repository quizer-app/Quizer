using MediatR;
using Quizer.Application.Authentication.Common;

namespace Quizer.Application.Authentication.Commands
{
    public record RegisterCommand(
        string FirstName,
        string LastName,
        string Email,
        string Password
        ) : IRequest<AuthenticationResult>;
}
