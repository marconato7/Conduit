using Conduit.Application.Articles.Shared;

namespace Conduit.Application.Articles.UnfavoriteArticle;

public sealed record UnfavoriteArticleCommandDto
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
