namespace Conduit.Api.Endpoints.Articles.UnfavoriteArticle;

internal sealed record UnfavoriteArticleResponseProps
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
