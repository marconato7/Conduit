namespace Conduit.Api.Endpoints.Articles.UpdateArticle;

internal sealed record UpdateArticleRequestProps
(
    string? Title,
    string? Description,
    string? Body
);
