using Conduit.Application.Abstractions.Cqrs;
using Conduit.Domain.Articles;

namespace Conduit.Application.Articles.GetArticle;

public sealed record GetArticleQuery(string Slug)
    : IQuery<Article>;
// ) : IQuery<ArticleResponse>; // refactor?
