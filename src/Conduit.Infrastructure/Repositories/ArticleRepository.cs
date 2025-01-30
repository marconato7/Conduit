using Conduit.Domain.Articles;
using Conduit.Infrastructure.Data;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Infrastructure.Repositories;

public sealed class ArticleRepository(ApplicationDbContext applicationDbContext) : IArticleRepository
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public void Add(Article article)
    {
        _applicationDbContext.Add(article);
    }

    public async Task<Article?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var article = await _applicationDbContext
            .Set<Article>()
            .SingleOrDefaultAsync(article => article.Id == id, cancellationToken: cancellationToken);

        return article;
    }

    public async Task<Article?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var article = await _applicationDbContext
            .Set<Article>()
            .SingleOrDefaultAsync(article => article.Slug == slug, cancellationToken: cancellationToken);

        return article;
    }

    public async Task<IEnumerable<Article>> ListArticles(CancellationToken cancellationToken = default)
    {
        var articles = await _applicationDbContext
            .Set<Article>()
            .ToListAsync(cancellationToken: cancellationToken);

        return articles;
    }

    public async Task<Result> RemoveBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var article = await _applicationDbContext
            .Set<Article>()
            .SingleOrDefaultAsync
            (
                article => article.Slug == slug,
                cancellationToken: cancellationToken
            );

        if (article is not null)
        {
            _applicationDbContext
                .Set<Article>()
                .Remove(article);
        }

        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
