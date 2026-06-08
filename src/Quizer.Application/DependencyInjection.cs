using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quizer.Application.Common.Behaviors;
using Quizer.Application.Common.Cqrs;
using Quizer.Application.Services;
using System.Reflection;

namespace Quizer.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, ConfigurationManager configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddScoped<IRequestDispatcher, RequestDispatcher>();
        services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
        RegisterClosedImplementations(services, assembly, typeof(IRequestHandler<,>));
        RegisterClosedImplementations(services, assembly, typeof(IDomainEventHandler<>));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        services.AddValidatorsFromAssembly(assembly);

        services.AddServices(configuration);

        //services
        //    .AddFluentEmail("quizer@elotoja.com")
        //    .AddSendGridSender("API_KEY");

        return services;
    }

    private static void RegisterClosedImplementations(
        IServiceCollection services,
        Assembly assembly,
        Type openServiceType)
    {
        var implementationTypes = assembly.GetTypes()
            .Where(type => type is { IsAbstract: false, IsInterface: false });

        foreach (var implementationType in implementationTypes)
        {
            var serviceTypes = implementationType.GetInterfaces()
                .Where(type => type.IsGenericType && type.GetGenericTypeDefinition() == openServiceType);

            foreach (var serviceType in serviceTypes)
            {
                services.AddScoped(serviceType, implementationType);
            }
        }
    }
}
