using FluentResults;

namespace Conduit.Domain.Articles;

public interface IArticleRepository
{
    Task<Article?> GetByIdAsync      (Guid   id,   CancellationToken cancellationToken = default);
    Task<Article?> GetBySlugAsync    (string slug, CancellationToken cancellationToken = default);
    Task<Result> RemoveBySlugAsync   (string slug, CancellationToken cancellationToken = default);
    Task<IEnumerable<Article>> ListArticles (CancellationToken cancellationToken = default);
    void Add(Article article);
}
