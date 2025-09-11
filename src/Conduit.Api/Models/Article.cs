namespace Conduit.Api.Models;

internal sealed class Article
{
    public Guid Id { get; private set; }
    private readonly List<Tag> _tags = [];
    public IReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();
}
