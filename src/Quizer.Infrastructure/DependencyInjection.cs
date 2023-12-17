using Microsoft.Extensions.DependencyInjection;
using Quizer.Application.Common.Interfaces.Authentication;
using Quizer.Infrastructure.Authentication;

namespace Quizer.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfractructure(this IServiceCollection services)
        {
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            return services;
        }
    }
}
