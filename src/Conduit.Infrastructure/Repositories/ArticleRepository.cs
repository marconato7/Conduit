using Conduit.Domain.Articles;
using Conduit.Domain.Tags;
using Conduit.Domain.Users;
using Conduit.Infrastructure.Data;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Infrastructure.Repositories;

public sealed class ArticleRepository(ApplicationDbContext applicationDbContext) : IArticleRepository
{
    private const int PAGINATION_CONSTANTS_LIMIT  = 20;
    private const int PAGINATION_CONSTANTS_OFFSET = 0;

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
            .Include(article => article.Author)
            .SingleOrDefaultAsync(article => article.Slug == slug, cancellationToken: cancellationToken);

        return article;
    }

    public async Task<List<Tag>?> GetTagsByName(List<string> tagNames, CancellationToken cancellationToken = default)
    {
        var existingTags = await _applicationDbContext
        .Set<Tag>()
        .Where(tag => tagNames.Contains(tag.Name))
        .ToListAsync(cancellationToken: cancellationToken);

        return existingTags;
    }

    public async Task<IEnumerable<Article>> ListArticles
    (
        string?           tag,
        string?           author,
        string?           favorited,
        int?              limit,
        int?              offset,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<Article> query = _applicationDbContext.Set<Article>();
        Tag? existingTag          = null;
        User? existingUser        = null;

        query = query
            .Include(article => article.TagList)
            .Include(article => article.Author)
            .Include(article => article.UsersThatFavorited)
            .OrderByDescending(article => article.CreatedAtUtc)
            .Skip(offset ?? PAGINATION_CONSTANTS_OFFSET)
            .Take(limit ?? PAGINATION_CONSTANTS_LIMIT);

        if (tag is not null)
        {
            existingTag = await _applicationDbContext
                .Set<Tag>()
                .FirstOrDefaultAsync
                (
                    t => t.Name == tag,
                    cancellationToken: cancellationToken
                );

            if (existingTag is not null)
            {
                query = query.Where
                (
                    a => a.TagList.Contains(existingTag)
                );
            }
        }

        if (author is not null)
        {
            query = query.Where
            (
                article => article.Author.Username == author
            );
        }

        if (favorited is not null)
        {
            existingUser = await _applicationDbContext
                .Set<User>()
                .FirstOrDefaultAsync
                (
                    u => u.Username == favorited,
                    cancellationToken: cancellationToken
                );

            if (existingUser is not null)
            {
                query = query.Where
                (
                    article => article.UsersThatFavorited.Contains(existingUser)
                );
            }
        }

        var articles = await query.ToListAsync
        (
            cancellationToken: cancellationToken
        );

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
