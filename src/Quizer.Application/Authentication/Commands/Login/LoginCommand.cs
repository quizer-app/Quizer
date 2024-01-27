using ErrorOr;
using MediatR;

namespace Quizer.Application.Authentication.Commands.Login;

public record LoginCommand(
    string Email,
    string Password
    ) : IRequest<ErrorOr<LoginResult>>;
