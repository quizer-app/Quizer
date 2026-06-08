using ErrorOr;
using Quizer.Application.Common.Cqrs;

namespace Quizer.Application.Authentication.Commands.Logout;

public record LogoutCommand(
    string RefreshToken
    ) : ICommand<ErrorOr<LogoutResult>>;
