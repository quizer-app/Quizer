namespace Quizer.Application.Users.Common;

public record UserResult (
    Guid UserId,
    string Username,
    string Email,
    DateTime CreatedAt);
