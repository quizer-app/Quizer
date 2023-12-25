using Microsoft.AspNetCore.Mvc.Infrastructure;
using Quizer.Api.Common.Errors;
using Quizer.Api.Common.Mapping;

namespace Quizer.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddSingleton<ProblemDetailsFactory, QuizerProblemDetailsFactory>();

        services.AddMappings();

        return services;
    }
}
