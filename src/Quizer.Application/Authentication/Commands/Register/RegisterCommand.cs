using ErrorOr;
using MediatR;

namespace Quizer.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string Username,
    string Email,
    string Password
    ) : IRequest<ErrorOr<RegisterResult>>;
