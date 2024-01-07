using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Quizer.Application.Common.Behaviors;
using Quizer.Application.Services;
using System.Reflection;

namespace Quizer.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        services.AddValidatorsFromAssembly(assembly);

        services.AddServices();

        services
            .AddFluentEmail("quizer@elotoja.com")
            .AddSendGridSender("API_KEY");

        return services;
    }
}
