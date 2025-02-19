using Conduit.Domain.Users;
using FluentResults;

namespace Conduit.Domain.Articles;

public interface IArticleRepository
{
    Task<List<Article>?> FeedArticles
    (
        User              user,
        CancellationToken cancellationToken = default
    );

    Task<Article?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Article?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    
    Task<Result> RemoveBySlugAsync(string slug, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<Article>> ListArticles
    (
        string? tag = null,
        string? author = null,
        string? favorited = null,
        int? limit = 20,
        int? offset = 0,
        CancellationToken cancellationToken = default
    );

    Task<List<Tag>?> GetTagsByName(List<string> tagNames, CancellationToken cancellationToken = default);

    void Add(Article article);

    Task<Result> StoreArticleWithComment
    (
        Article           article,
        Comment           comment,
        CancellationToken cancellationToken = default
    );

    Task<Article?> GetBySlugWithCommentsAsync(string slug, CancellationToken cancellationToken = default);

    Task<Article?> GetArticleQueryAsync
    (
        string slug,
        CancellationToken cancellationToken = default
    );
}
