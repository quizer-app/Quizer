using Quizer.Api.Common.Settings;
using Scalar.AspNetCore;
using Serilog;

namespace Quizer.Api;

public static class ApplicationConfiguration
{
    public static WebApplication UsePresentation(this WebApplication app, ConfigurationManager configuration)
    {
        app.UseVersionedSwagger(configuration);

        app.UseSerilogRequestLogging();

        app.UseExceptionHandler("/api/v1/errors");

        app.UseHttpsRedirection();

        app.UseCors(CorsSettings.PolicyName);

        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapFallbackToFile("index.html");
        app.Run();

        return app;
    }

    private static WebApplication UseVersionedSwagger(this WebApplication app, ConfigurationManager configuration)
    {
        var swaggerSettings = new SwaggerSettings();
        configuration.Bind(SwaggerSettings.SectionName, swaggerSettings);

        if (!swaggerSettings.Enabled)
        {
            return app;
        }

        app.MapOpenApi();
        app.MapScalarApiReference();

        return app;
    }
}
