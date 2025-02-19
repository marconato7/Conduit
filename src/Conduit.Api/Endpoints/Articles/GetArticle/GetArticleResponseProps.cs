namespace Conduit.Api.Endpoints.Articles.GetArticle;

internal sealed record GetArticleResponseProps
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
