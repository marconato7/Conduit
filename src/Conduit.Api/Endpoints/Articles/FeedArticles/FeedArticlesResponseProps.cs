namespace Conduit.Api.Endpoints.Articles.FeedArticles;

public sealed record FeedArticlesResponseProps
(
    string                 Slug,
    string                 Title,
    string                 Description,
    List<string>?          TagList,
    DateTime               CreatedAt,
    DateTime?              UpdatedAt,
    bool                   Favorited,
    int                    FavoritesCount,
    AuthorModelForResponse Author
);

public sealed record AuthorModelForResponse
(
    string  Username,
    string? Bio,
    string? Image,
    bool    Following
);
