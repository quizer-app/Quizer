using Diacritics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quizer.Application.Common.Settings;
using Quizer.Application.Services.Image;
using Quizer.Application.Services.Slugify;

namespace Quizer.Application.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<ImagesSettings>(configuration.GetSection(ImagesSettings.SectionName));

        services.AddHttpClient<IImageService, ImageService>();
        services.AddTransient<IImageService, ImageService>();

        services.AddSingleton<IDiacriticsMapper, DefaultDiacriticsMapper>();
        services.AddSingleton<ISlugifyService, SlugifyService>();
        //services.AddSingleton<IEmailService, EmailService>();

        return services;
    }
}

