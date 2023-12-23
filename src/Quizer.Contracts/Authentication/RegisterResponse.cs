namespace Quizer.Contracts.Authentication
{
    public record RegisterResponse(
        Guid Id,
        string UserName,
        string Email
    );
}
