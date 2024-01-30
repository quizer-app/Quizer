namespace Quizer.Contracts.User;

public record UserResponse(
    string UserId,
    string Username,
    string Email,
    DateTime CreatedAt);
