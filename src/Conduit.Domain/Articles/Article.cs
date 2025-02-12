using Conduit.Domain.Abstractions;
using Conduit.Domain.Articles.Events;
using Conduit.Domain.Tags;
using Conduit.Domain.Users;
using Slugify;

namespace Conduit.Domain.Articles;

public sealed class Article : Entity
{
    public string     Title              { get; private set; } // refactor: solve primitive obsession
    public string     Description        { get; private set; } // refactor: solve primitive obsession
    public string     Body               { get; private set; } // refactor: solve primitive obsession
    public List<Tag>  TagList            { get; private set; } // refactor: solve primitive obsession
    public DateTime   CreatedAtUtc       { get; private set; } // refactor: solve primitive obsession
    public DateTime?  UpdatedAtUtc       { get; private set; } // refactor: solve primitive obsession
    public string     Slug               { get; private set; } // refactor: solve primitive obsession
    public Guid       AuthorId           { get; private set; }
    public User       Author             { get; private set; } = null!;
    public List<User> UsersThatFavorited { get; }              = [];

    private Article() {} // for EF Core

    private Article
    (
        string    title,
        string    description,
        string    body,
        List<Tag> tagList,
        User      author,
        DateTime  createdAtUtc,
        string    slug
    )
    {
        // refactor: add validation
        Title        = title;
        Description  = description;
        Body         = body;
        TagList      = tagList;
        Slug         = title;
        Id           = Guid.CreateVersion7();
        Author       = author;
        AuthorId     = author.Id;
        CreatedAtUtc = createdAtUtc;
    }

    public static Article Create
    (
        string        title,
        string        description,
        string        body,
        User          author,
        DateTime      createdAtUtc,
        List<string>? tagsThatNeedToBeCreated,
        List<Tag>?    existingTags
    )
    {
        SlugHelper helper = new();

        // refactor: add validation
        List<Tag> tags = [];

        if (tagsThatNeedToBeCreated is not null)
        {
            foreach (var tagName in tagsThatNeedToBeCreated)
            {
                tags.Add(Tag.Create(tagName));
            }
        }

        if (existingTags is not null)
        {
            tags.AddRange(existingTags);
        }
        
        var article = new Article
        (
            title:        title,
            description:  description,
            body:         body,
            tagList:      tags,
            author:       author,
            createdAtUtc: createdAtUtc,
            slug:         helper.GenerateSlug(title)

        );

        article.RaiseDomainEvent(new ArticleCreatedDomainEvent(article.Id));

        return article;
    }
}
