namespace Quizer.Application.Services.Authentication
{
    public interface IAuthenticationCommandService
    {
        Task<AuthenticationResult> Login(string email, string password);
        Task<AuthenticationResult> Register(string firstName, string lastName, string email, string password);
    }
}