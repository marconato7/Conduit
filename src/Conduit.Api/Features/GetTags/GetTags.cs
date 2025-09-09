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
                async (ApplicationDbContext applicationDbContext) =>
                {
                    var tags = await applicationDbContext.Set<Tag>()
                        .ToListAsync();

                    var getTagsResponse = new GetTagsResponse(
                        [.. tags.Select(t => t.Name)]
                    );

                    return Results.Ok(getTagsResponse);
                }
            )
            .AllowAnonymous();
    }
}
