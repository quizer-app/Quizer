using Quizer.Domain.UserAggregate;

namespace Quizer.Application.Authentication.Commands.Login;

public record LoginResult(
    User User,
    string AccessToken,
    string RefreshToken
);
