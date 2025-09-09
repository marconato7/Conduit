using Conduit.Domain.Abstractions;
using Conduit.Domain.Articles.Events;
using Conduit.Domain.Users;
using Slugify;

namespace Conduit.Domain.Articles;

public sealed class Article : Entity
{
    public string        Title              { get; private set; } // refactor: solve primitive obsession
    public string        Description        { get; private set; } // refactor: solve primitive obsession
    public string        Body               { get; private set; } // refactor: solve primitive obsession
    public List<Tag>     Tags               { get; private set; } // refactor: solve primitive obsession
    public DateTime      CreatedAtUtc       { get; private set; } // refactor: solve primitive obsession
    public DateTime      UpdatedAtUtc       { get; private set; } // refactor: solve primitive obsession
    public string        Slug               { get; private set; } // refactor: solve primitive obsession
    public Guid          AuthorId           { get; private set; }
    public User          Author             { get; private set; } = null!;
    public List<User>    UsersThatFavorited { get; }              = [];
    public List<Comment> Comments           { get; private set; } = []; // refactor: solve primitive obsession

    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Article() {} // for EF Core
    #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

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
        Tags         = tagList;
        Slug         = slug;
        Id           = Guid.CreateVersion7();
        Author       = author;
        AuthorId     = author.Id;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = createdAtUtc;
    }

    public static Article Create
    (
        string       title,
        string       description,
        string       body,
        User         author,
        DateTime     createdAtUtc,
        List<string> tagsThatNeedToBeCreated,
        List<Tag>?   existingTags
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

    public void AddComment(Comment comment)
    {
        if (Comments.Contains(comment))
        {
            return;
        }

        Comments.Add(comment);

        return;
    }

    public void AddComments(List<Comment> comments)
    {
        Comments.AddRange(comments);
        return;
    }

    public void RemoveComment(Guid commentId)
    {
        var comment = Comments.Find
        (
            comment => comment.Id == commentId
        );

        if (comment is not null)
        {
            Comments.Remove(comment);
        }
    }

    public void Update
    (
        string? title,
        string? description,
        string? body
    )
    {
        SlugHelper helper = new();
        Title             = title       ?? Title;
        Description       = description ?? Description;
        Body              = body        ?? Body;
        Slug              = helper.GenerateSlug(title ?? Title);
        UpdatedAtUtc      = DateTime.UtcNow;
    }
}
