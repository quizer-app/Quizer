using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Quizer.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR();

            return services;
        }
    }
}
