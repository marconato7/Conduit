using Conduit.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Api.Extensions;

internal static class ApplicationBuilderExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        using var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        applicationDbContext.Database.EnsureDeleted();
        applicationDbContext.Database.EnsureCreated();
        // applicationDbContext.Database.Migrate();
    }
}
