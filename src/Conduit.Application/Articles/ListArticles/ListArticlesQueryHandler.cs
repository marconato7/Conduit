using Conduit.Application.Abstractions.Cqrs;
using Conduit.Domain.Articles;
using FluentResults;

namespace Conduit.Application.Articles.ListArticles;

internal sealed class ListArticlesQueryHandler(IArticleRepository articleRepository)
    : ICommandHandler<ListArticlesQuery, ListArticlesQueryDto>
{
    private readonly IArticleRepository _articleRepository = articleRepository;

    public async Task<Result<ListArticlesQueryDto>> Handle
    (
        ListArticlesQuery query,
        CancellationToken cancellationToken
    )
    {
        var articles = await _articleRepository.ListArticles(cancellationToken);

        return new ListArticlesQueryDto(articles);
    }
}
