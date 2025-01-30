using Conduit.Domain.Abstractions;
using Conduit.Domain.Articles.Events;
using Conduit.Domain.Users;

namespace Conduit.Domain.Articles;

public sealed class Article : Entity
{
    public string     Title              { get; private set; } // refactor: solve primitive obsession
    public string     Description        { get; private set; } // refactor: solve primitive obsession
    public string     Body               { get; private set; } // refactor: solve primitive obsession
    public string[]?  TagList            { get; private set; } // refactor: solve primitive obsession
    public DateTime   CreatedAtUtc       { get; private set; } // refactor: solve primitive obsession
    public DateTime?  UpdatedAtUtc       { get; private set; } // refactor: solve primitive obsession
    public string     Slug               { get; private set; } // refactor: solve primitive obsession
    public Guid       AuthorId           { get; private set; }
    public User       Author             { get; private set; } = null!;
    public List<User> UsersThatFavorited { get; }              = [];

    private Article() {} // for EF Core

    public Article
    (
        string title,
        string description,
        string body
    )
    {
        // refactor: add validation
        Title       = title;
        Description = description;
        Body        = body;
        Slug        = title;
        Id          = Guid.CreateVersion7();
    }

    public static Article Create
    (
        string    title,
        string    description,
        string    body,
        string[]? tagList
    )
    {
        // refactor: add validation
        var article = new Article
        (
            title:       title,
            description: description,
            body:        body
        );

        article.RaiseDomainEvent(new ArticleCreatedDomainEvent(article.Id));

        return article;
    }
}
