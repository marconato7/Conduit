using Conduit.Application.Abstractions.Cqrs;

namespace Conduit.Application.Articles.ListArticles;

public sealed record ListArticlesQuery
(
    string?           CurrentUsersEmail = null,
    string?           Tag               = null,
    string?           Author            = null,
    string?           Favorited         = null,
    int?              Limit             = null,
    int?              Offset            = null
) : ICommand<ListArticlesQueryDto>;
