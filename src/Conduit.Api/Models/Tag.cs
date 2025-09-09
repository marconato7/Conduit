namespace Conduit.Api.Models;

internal sealed class Tag
{
    public Guid   Id   { get; private set; }
    public string Name { get; private set; }

    public Tag(string name)
    {
        Id   = Guid.CreateVersion7();
        Name = name;
    }
}
