namespace Conduit.Api.Endpoints.Articles.ListArticles;

public sealed record ListArticlesRequestProps
(
    string        Title,
    string        Description,
    string        Body,
    List<string>? TagList
);
