namespace Conduit.Api.Endpoints.Articles.FeedArticles;

public sealed record FeedArticlesRequestProps
(
    string        Title,
    string        Description,
    string        Body,
    List<string>? TagList
);
