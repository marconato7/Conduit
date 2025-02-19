using Conduit.Application.Abstractions.Cqrs;

namespace Conduit.Application.Articles.GetArticle;

public sealed record GetArticleQuery
(
    string  Slug,
    string? CurrentUsersEmail = null
) : IQuery<GetArticleQueryDto>;
