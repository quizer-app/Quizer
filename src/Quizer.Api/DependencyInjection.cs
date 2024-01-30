using Microsoft.AspNetCore.Mvc.Infrastructure;
using Quizer.Api.Common.Errors;
using Quizer.Api.Common.Mapping;
using Asp.Versioning;
using Microsoft.Extensions.Options;
using Quizer.Api.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using Quizer.Api.Common.Settings;

namespace Quizer.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var corsSettings = new CorsSettings();
        configuration.Bind(CorsSettings.SectionName, corsSettings);

        services.AddApiVersioning();

        services.AddControllers();
        services.AddSingleton<ProblemDetailsFactory, QuizerProblemDetailsFactory>();

        services.AddMappings();

        services.AddCors(options =>
        {
            options.AddPolicy(
                name: CorsSettings.PolicyName,
                policy =>
                {
                    policy
                        .WithOrigins(corsSettings.Origins)
                        .AllowCredentials()
                        .AllowAnyHeader();
                });
        });

        return services;
    }

    private static IServiceCollection AddApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddMvc().AddApiExplorer();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());

        return services;
    }
}
