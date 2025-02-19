using Conduit.Application.Abstractions.Cqrs;
using Conduit.Application.Abstractions.Data;
using Dapper;
using FluentResults;

namespace Conduit.Application.Articles.GetTags;

internal sealed class GetTagsQueryHandler
(
    ISqlConnectionFactory sqlConnectionFactory
)   : IQueryHandler<GetTagsQuery, GetTagsQueryDto>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;

    public async Task<Result<GetTagsQueryDto>> Handle(GetTagsQuery query, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        var sql = "SELECT name AS Name FROM tags";

        var tags = await connection.QueryAsync<string>(sql);

        if (tags is null)
        {
            return Result.Fail("something went wrong");
        }

        var getTagsQueryDto = new GetTagsQueryDto
        (
            [.. tags.Select(tag => tag)]
        );

        return getTagsQueryDto;
    }
}
