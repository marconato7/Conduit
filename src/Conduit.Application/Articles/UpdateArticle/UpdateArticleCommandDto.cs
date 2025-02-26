using Conduit.Application.Articles.Shared;

namespace Conduit.Application.Articles.UpdateArticle;

public sealed record UpdateArticleCommandDto
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
