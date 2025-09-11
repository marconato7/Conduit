using Conduit.Api.Data;
using Conduit.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Api.Features.GetTags;

public static partial class GetTags
{
    public static void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder
            .MapGet
            (
                "api/tags",
                async (
                    ApplicationDbContext applicationDbContext,
                    CancellationToken cancellationToken
                ) =>
                {
                    var tagsArray = await applicationDbContext
                        .Set<Tag>()
                        .AsNoTracking()
                        .Select(tag => tag.Name)
                        .ToArrayAsync(cancellationToken: cancellationToken);

                    var getTagsResponse = new GetTagsResponse(tagsArray);

                    return Results.Ok(getTagsResponse);
                }
            )
            .AllowAnonymous();
    }
}
