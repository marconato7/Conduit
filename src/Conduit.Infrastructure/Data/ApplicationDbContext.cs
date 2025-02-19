using System.Text.Json;
using Conduit.Domain.Abstractions;
using Conduit.Domain.Articles;
using Conduit.Domain.Users;
using Conduit.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Infrastructure.Data;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Article>       Articles       { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<User>          Users          { get; set; }
    public DbSet<Tag>           Tags           { get; set; }
    public DbSet<Comment>       Comments       { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    // public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    // {
    //     try
    //     {
    //         var outboxMessages = ChangeTracker
    //             .Entries<Entity>()
    //             .Select(entry => entry.Entity) 
    //             .SelectMany(entity =>
    //             {
    //                 var domainEvents = entity.GetDomainEvents();
    //                 entity.ClearDomainEvents();
    //                 return domainEvents;
    //             })
    //             .Select(domainEvent =>
    //             {
    //                 return new OutboxMessage
    //                 (
    //                     type: domainEvent.GetType().Name,
    //                     content: JsonSerializer.Serialize(domainEvent, domainEvent.GetType())
    //                 );
    //             })
    //             .ToList();

    //         Set<OutboxMessage>().AddRange(outboxMessages);

    //         var result = await base.SaveChangesAsync(cancellationToken);

    //         return result;
    //     }
    //     catch (Exception exception)
    //     {
    //         throw new Exception(nameof(exception), exception);
    //     }
    // }
}
