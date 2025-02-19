namespace Conduit.Domain.Articles;

public sealed class Tag
{
    public Guid           Id           { get; private set; } // refactor: solve primitive obsession
    public string         Name         { get; private set; } // refactor: solve primitive obsession
    public DateTime       CreatedAtUtc { get; private set; } // refactor: solve primitive obsession
    public DateTime?      UpdatedAtUtc { get; private set; } // refactor: solve primitive obsession
    public List<Article>? Articles     { get; private set; } // refactor: solve primitive obsession

    #pragma warning disable CS8618
    private Tag() {} // for EF Core
    #pragma warning restore CS8618

    private Tag(string name)
    {
        // refactor: add validation
        Name = name; 
        Id   = Guid.CreateVersion7();
    }

    public static Tag Create(string name)
    {
        // refactor: add validation
        var tag = new Tag(name);
        return tag;
    }
}
