using ErrorOr;
using MediatR;

namespace Quizer.Application.Authentication.Commands.Logout;

public record LogoutCommand(
    string RefreshToken
    ) : IRequest<ErrorOr<LogoutResult>>;
