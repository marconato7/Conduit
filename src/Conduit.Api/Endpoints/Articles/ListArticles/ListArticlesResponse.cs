namespace Conduit.Api.Endpoints.Articles.ListArticles;

public sealed record ListArticlesResponse
(
    List<ListArticlesResponseProps> Articles,
    int ArticlesCount
);
