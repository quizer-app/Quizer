using Quizer.Api.Common.Settings;
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

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.Run();

        return app;
    }

    private static WebApplication UseVersionedSwagger(this WebApplication app, ConfigurationManager configuration)
    {
        var swaggerSettings = new SwaggerSettings();
        configuration.Bind(SwaggerSettings.SectionName, swaggerSettings);

        if (swaggerSettings.Enabled)
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in app.DescribeApiVersions())
                    {
                        options.SwaggerEndpoint(
                            $"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName);
                    }
                });
        }

        return app;
    }
}
