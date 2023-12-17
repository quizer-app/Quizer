using Microsoft.Extensions.DependencyInjection;
using Quizer.Application.Services.Authentication.Commands;
using Quizer.Application.Services.Authentication.Queries;

namespace Quizer.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationQueryService, AuthenticationQueryService>();
            services.AddScoped<IAuthenticationCommandService, AuthenticationCommandService>();

            return services;
        }
    }
}
