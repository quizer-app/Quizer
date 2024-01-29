namespace Quizer.Contracts.Authentication;

public record LoginResponse(
    Guid Id,
    string UserName,
    string Email,
    string AccessToken
);
