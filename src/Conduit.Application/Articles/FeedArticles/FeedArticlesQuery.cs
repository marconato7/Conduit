using Conduit.Application.Abstractions.Cqrs;

namespace Conduit.Application.Articles.FeedArticles;

public sealed record FeedArticlesQuery
(
    string?           CurrentUsersEmail = null,
    string?           Tag               = null,
    string?           Author            = null,
    string?           Favorited         = null,
    int?              Limit             = null,
    int?              Offset            = null
) : ICommand<FeedArticlesQueryDto>;
