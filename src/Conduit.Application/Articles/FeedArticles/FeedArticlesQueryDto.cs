using Conduit.Application.Articles.Shared;

namespace Conduit.Application.Articles.FeedArticles;

public sealed record FeedArticlesQueryDto
(
    List<FeedArticlesQueryDtoProps> Articles,
    int ArticlesCount
);

public sealed record FeedArticlesQueryDtoProps
(
    string        Slug,
    string        Title,
    string        Description,
    List<string>  TagList,
    DateTime      CreatedAt,
    DateTime?     UpdatedAt,
    bool          Favorited,
    int           FavoritesCount,
    AuthorModel   Author
);
