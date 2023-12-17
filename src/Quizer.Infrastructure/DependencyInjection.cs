using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quizer.Application.Common.Interfaces.Authentication;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Application.Common.Interfaces.Services;
using Quizer.Infrastructure.Authentication;
using Quizer.Infrastructure.Persistance;
using Quizer.Infrastructure.Services;

namespace Quizer.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfractructure(
            this IServiceCollection services,
            ConfigurationManager configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            services.AddScoped<IUserRepository, UserRepository>();

            services
                .AddDbContext<QuizerDbContext>(options =>
                    options.UseNpgsql(
                        configuration.GetConnectionString("DefaultConnection"))
                    );

            return services;
        }
    }
}
