using ErrorOr;
using Quizer.Application.Common.Cqrs;

namespace Quizer.Application.Authentication.Commands.Login;

public record LoginCommand(
    string Email,
    string Password
    ) : ICommand<ErrorOr<LoginResult>>;
