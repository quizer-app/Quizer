using Quizer.Application.Services.Authentication.Common;

namespace Quizer.Application.Services.Authentication.Queries
{
    public interface IAuthenticationQueryService
    {
        Task<AuthenticationResult> Login(string email, string password);
    }
}