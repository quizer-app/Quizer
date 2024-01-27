using ErrorOr;
using MediatR;

namespace Quizer.Application.Authentication.Commands.RefreshToken;

public record RefreshTokenCommand(
    string RefreshToken
    ) : IRequest<ErrorOr<string>>;
