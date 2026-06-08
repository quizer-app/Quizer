using ErrorOr;
using Quizer.Application.Common.Cqrs;

namespace Quizer.Application.Authentication.Commands.RefreshToken;

public record RefreshTokenCommand(
    string RefreshToken
    ) : ICommand<ErrorOr<string>>;
