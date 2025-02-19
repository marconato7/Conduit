using Conduit.Domain.Abstractions;
using Conduit.Domain.Users;

namespace Conduit.Domain.Articles;

public sealed class Comment
{
    public Guid          Id           { get; private set; } // refactor: solve primitive obsession
    public string        Body         { get; private set; } // refactor: solve primitive obsession
    public DateTime      CreatedAtUtc { get; private set; } // refactor: solve primitive obsession
    public DateTime      UpdatedAtUtc { get; private set; } // refactor: solve primitive obsession
    public Guid          AuthorId     { get; private set; }
    public User          Author       { get; private set; } = null!;
    public Article       Article      { get; private set; } = null!;

    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Comment() {} // for EF Core
    #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    private Comment
    (
        string   body,
        DateTime createdAtUtc,
        User     author
    )
    {
        // refactor: add validation
        Body         = body;
        Id           = Guid.CreateVersion7();
        Author       = author;
        AuthorId     = author.Id;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = createdAtUtc;
    }

    public static Comment Create
    (
        string   body,
        User     author,
        DateTime createdAtUtc
    )
    {
        var comment = new Comment
        (
            body:         body,
            author:       author,
            createdAtUtc: createdAtUtc
        );

        // comment.RaiseDomainEvent(new CommentCreatedDomainEvent(comment.Id));

        return comment;
    }
}
