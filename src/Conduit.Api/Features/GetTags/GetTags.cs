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
                    var tagsArray = await applicationDbContext
                        .Set<Tag>()
                        .Select(tag => tag.Name)
                        .ToArrayAsync();

                    var getTagsResponse = new GetTagsResponse(tagsArray);

                    return Results.Ok(getTagsResponse);
                }
            )
            .AllowAnonymous();
    }
}
