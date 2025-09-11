using Conduit.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Api.Data;

internal sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<Tag>     Tags     => Set<Tag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>(appUser =>
        {
            appUser.Property(p => p.EnableNotifications).HasDefaultValue(true);
            appUser.Property(p => p.Initials).HasMaxLength(5);
        });

        modelBuilder.HasDefaultSchema("conduit");
    }
}
