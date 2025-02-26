namespace Conduit.Api.Endpoints.Articles.UpdateArticle;

internal sealed record UpdateArticleResponseProps
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
