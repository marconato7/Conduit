using Conduit.Api.Data;
using Conduit.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Api.Features.GetTagById;

public static partial class GetTagById
{
    public static void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder
            .MapGet
            (
                "api/tags/{id:Guid}",
                async (
                    ApplicationDbContext applicationDbContext,
                    Guid id,
                    CancellationToken cancellationToken
                ) =>
                {
                    var tag = await applicationDbContext
                        .Set<Tag>()
                        .AsNoTracking()
                        .SingleOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);

                    if (tag is null)
                    {
                        return Results.NotFound();
                    }

                    return Results.Ok(new TagDto(tag.Id, tag.Name));
                }
            )
            .AllowAnonymous()
            .WithName("GetTagById");
    }
}
