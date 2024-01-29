using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quizer.Infrastructure.Persistance;

namespace Quizer.Infrastructure;

public static class MigrationExtensions
{
    public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<QuizerDbContext>();
        context.Database.Migrate();

        return app;
    }
}
