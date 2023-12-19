using ErrorOr;
using MediatR;
using Quizer.Application.Authentication.Common;

namespace Quizer.Application.Authentication.Queries
{
    public record LoginQuery(
        string Email,
        string Password
        ) : IRequest<ErrorOr<AuthenticationResult>>;
}
