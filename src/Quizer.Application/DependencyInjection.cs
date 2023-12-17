using Microsoft.Extensions.DependencyInjection;
using Quizer.Application.Services.Authentication;

namespace Quizer.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            return services;
        }
    }
}
