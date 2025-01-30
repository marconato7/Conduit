namespace Conduit.Application.Articles.FavoriteArticle;

public sealed record FavoriteArticleCommandDto
(
    string      Slug,
    string      Title,
    string      Description,
    string      Body,
    string[]?   TagList,
    DateTime    CreatedAt,
    DateTime?   UpdatedAt,
    bool        Favorited,
    int         FavoritesCount,
    AuthorModel Author
);

public sealed record AuthorModel
(
    string  Username,
    string? Bio,
    string? Image,
    bool    Following
);
