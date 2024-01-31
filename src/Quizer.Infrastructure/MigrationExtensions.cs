using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quizer.Domain.UserAggregate;
using Quizer.Infrastructure.Authentication;
using Quizer.Infrastructure.Persistance;
using System.Security.Claims;

namespace Quizer.Infrastructure;

public static class MigrationExtensions
{
    public static async Task MigrateDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<QuizerDbContext>();
        context.Database.Migrate();

        await app.SeedUsers();
    }

    public static async Task SeedUsers(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var adminUser = await userManager.FindByNameAsync("admin");
        if(adminUser is null)
        {
            adminUser = new User
            {
                UserName = "admin",
                Email = "admin@quizer.com",
            };
            await userManager.CreateAsync(adminUser, "Test123#");
        }

        var adminRole = await roleManager.FindByNameAsync("Administrator");
        if (adminRole is null)
        {
            adminRole = new IdentityRole("Administrator");
            await roleManager.CreateAsync(adminRole);

            await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaims.Permission, Permission.Admin.ToString()));
        }

        if (adminRole.Name is not null && !await userManager.IsInRoleAsync(adminUser, adminRole.Name))
        {
            await userManager.AddToRoleAsync(adminUser, adminRole.Name);
        }
    }
}
