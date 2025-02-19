namespace Conduit.Api.Endpoints.Comments.GetCommentsFromAnArticle;

internal sealed record GetCommentsFromAnArticleResponseProps
(
    Guid                   Id,
    DateTime               CreatedAt,
    DateTime               UpdatedAt,
    string                 Body,
    AuthorModelForResponse Author
);
