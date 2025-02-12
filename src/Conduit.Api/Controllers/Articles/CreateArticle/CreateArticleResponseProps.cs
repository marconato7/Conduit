namespace Conduit.Api.Controllers.Articles.CreateArticle;

public sealed record CreateArticleResponseProps
(
    string                 Slug,
    string                 Title,
    string                 Description,
    string                 Body,
    List<string>?          TagList,
    DateTime               CreatedAt,
    DateTime?              UpdatedAt,
    bool                   Favorited,
    int                    FavoritesCount,
    AuthorModelForResponse Author
);
