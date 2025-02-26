namespace Conduit.Api.Endpoints.Articles.FeedArticles;

public sealed record FeedArticlesResponse
(
    List<FeedArticlesResponseProps> Articles,
    int ArticlesCount
);
