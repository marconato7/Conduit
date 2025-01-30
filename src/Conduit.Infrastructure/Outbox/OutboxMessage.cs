namespace Conduit.Infrastructure.Outbox;

public sealed class OutboxMessage(string type, string content)
{
    public Guid      Id             { get; init; } = Guid.CreateVersion7();
    public string    Type           { get; init; } = type;
    public string    Content        { get; init; } = content;
    public DateTime  OccurredOnUtc  { get; init; } = DateTime.UtcNow;
    public DateTime? ProcessedOnUtc { get; init; }
    public string?   Error          { get; init; }
}
