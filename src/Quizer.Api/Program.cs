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

var swaggerSettings = new SwaggerSettings();
builder.Configuration.Bind(SwaggerSettings.SectionName, swaggerSettings);

var app = builder.Build();
{
    if (swaggerSettings.Enabled)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseSerilogRequestLogging();

    app.UseExceptionHandler("/error");
    
    app.UseHttpsRedirection();
    
    app.UseAuthentication();
    app.UseAuthorization();
    
    app.MapControllers();
    app.Run();
}

