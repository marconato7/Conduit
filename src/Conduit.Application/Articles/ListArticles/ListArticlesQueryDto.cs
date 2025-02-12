using Conduit.Application.Articles.Shared;

namespace Conduit.Application.Articles.ListArticles;

public sealed record ListArticlesQueryDto(List<ListArticlesQueryDtoProps> Articles, int ArticlesCount);

public sealed record ListArticlesQueryDtoProps
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
