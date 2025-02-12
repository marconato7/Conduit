namespace Conduit.Api.Endpoints.Comments.AddCommentsToAnArticle;

internal sealed record AddCommentsToAnArticleResponseProps
(
    Guid                   Id,
    DateTime               CreatedAtUtc,
    DateTime               UpdatedAt,
    string                 Body,
    AuthorModelForResponse Author
);
