using Quizer.Domain.UserAggregate;

namespace Quizer.Application.Authentication.Queries.Login;

public record LoginResult(
    User User,
    string AccessToken,
    string RefreshToken
);
