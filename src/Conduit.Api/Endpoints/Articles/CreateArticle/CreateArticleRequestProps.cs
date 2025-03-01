namespace Conduit.Api.Endpoints.Articles.CreateArticle;

public sealed record CreateArticleRequestProps
(
    string       Title,
    string       Description,
    string       Body,
    List<string> TagList
);
