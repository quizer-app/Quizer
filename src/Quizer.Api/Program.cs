using Quizer.Api;
using Quizer.Api.Common.Settings;
using Quizer.Application;
using Quizer.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Host.UseSerilog((context, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
    });
    builder.Services
        .AddApplication()
        .AddInfractructure(builder.Configuration)
        .AddPresentation();
}

Log.Logger =
    new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
.CreateLogger();

var app = builder.Build();
{
    var swaggerSettings = new SwaggerSettings();
    builder.Configuration.Bind(SwaggerSettings.SectionName, swaggerSettings);

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
    app.UseSerilogRequestLogging();

    app.UseExceptionHandler("/error");
    
    app.UseHttpsRedirection();
    
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.Run();
}

