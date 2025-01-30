using Conduit.Application.Abstractions.Cqrs;

namespace Conduit.Application.Articles.ListArticles;

public sealed record ListArticlesQuery
    : ICommand<ListArticlesQueryDto>;
