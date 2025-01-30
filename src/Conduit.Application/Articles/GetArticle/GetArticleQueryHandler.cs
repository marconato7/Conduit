using Conduit.Application.Abstractions;
using Conduit.Application.Abstractions.Cqrs;
using Conduit.Application.Abstractions.Data;
using Conduit.Domain.Articles;
using Dapper;
using FluentResults;

namespace Conduit.Application.Articles.GetArticle;

internal sealed class GetArticleQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    : IQueryHandler<GetArticleQuery, Article>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;

    public async Task<Result<Article>> Handle(GetArticleQuery query, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        var sql = 
        """
            SELECT
                id AS Id,
                title AS Title,
                description AS Description,
                body AS Body,
                slug AS Slug
            FROM articles
            WHERE slug = @Slug
        """;

        var article = await connection.QueryFirstOrDefaultAsync<Article>
        (
            sql,
            new
            {
                query.Slug
            }
        );

        if (article is null)
        {
            return Result.Fail($"Article with {query.Slug} not found");
        }

        return article;
    }
}
