namespace Conduit.Api.Endpoints.Articles.FavoriteArticle;

internal sealed record FavoriteArticleResponseProps
(
    string                 Slug,
    string                 Title,
    string                 Description,
    string                 Body,
    List<string>           TagList,
    DateTime               CreatedAt,
    DateTime?              UpdatedAt,
    bool                   Favorited,
    int                    FavoritesCount,
    AuthorModelForResponse Author
);
