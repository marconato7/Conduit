using Conduit.Application.Articles.Shared;

namespace Conduit.Application.Articles.FavoriteArticle;

public sealed record FavoriteArticleCommandDto
(
    string       Slug,
    string       Title,
    string       Description,
    string       Body,
    List<string> TagList,
    DateTime     CreatedAt,
    DateTime?    UpdatedAt,
    bool         Favorited,
    int          FavoritesCount,
    AuthorModel  Author
);
