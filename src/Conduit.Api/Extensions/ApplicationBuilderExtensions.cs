using Conduit.Api.Data;
using Conduit.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Conduit.Api.Extensions;

internal static class ApplicationBuilderExtensions
{
    public static async Task ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        using var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        applicationDbContext.Database.EnsureDeleted();

        applicationDbContext.Database.EnsureCreated();

        // applicationDbContext.Database.Migrate();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (!await roleManager.RoleExistsAsync(Roles.Admin))
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
        }

        if (!await roleManager.RoleExistsAsync(Roles.Member))
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Member));
        }

        // Tags
        List<string> tagsNames =
        [
            "php",
            "c#",
            "javascript",
            "java",
            "html",
            "css",
            "python",
            "sql",
            "mysql",
            "postgresql",
            "sqlite",
            "dns",
            "http",
        ];

        List<Tag> tags = [];

        foreach (var tagName in tagsNames)
        {
            tags.Add(new Tag(tagName));
        }

        await applicationDbContext.AddRangeAsync(tags);

        await applicationDbContext.SaveChangesAsync();
    }
}
