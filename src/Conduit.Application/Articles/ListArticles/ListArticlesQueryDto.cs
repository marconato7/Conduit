using Conduit.Domain.Articles;

namespace Conduit.Application.Articles.ListArticles;

public sealed record ListArticlesQueryDto(IEnumerable<Article> Articles);
