using Quizer.Application.Services.Authentication.Common;

namespace Quizer.Application.Services.Authentication.Commands
{
    public interface IAuthenticationCommandService
    {
        Task<AuthenticationResult> Register(string firstName, string lastName, string email, string password);
    }
}