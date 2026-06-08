using ErrorOr;
using Quizer.Application.Common.Cqrs;

namespace Quizer.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string Username,
    string Email,
    string Password
    ) : ICommand<ErrorOr<RegisterResult>>;
