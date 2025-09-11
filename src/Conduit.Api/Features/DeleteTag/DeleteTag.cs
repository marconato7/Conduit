using Conduit.Api.Data;
using Conduit.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Api.Features.DeleteTag;

public static partial class DeleteTag
{
    public static void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder
            .MapDelete
            (
                "api/tags/{tagId:Guid}",
                async (
                    ApplicationDbContext applicationDbContext,
                    Guid                 tagId,
                    CancellationToken    cancellationToken
                ) =>
                {
                    var tagToDelete = await applicationDbContext
                        .Set<Tag>()
                        .SingleOrDefaultAsync(
                            t => t.Id == tagId,
                            cancellationToken: cancellationToken
                        );

                    if (tagToDelete is null)
                    {
                        return Results.NotFound();
                    }

                    applicationDbContext.Remove(tagToDelete);

                    await applicationDbContext.SaveChangesAsync(cancellationToken);

                    return Results.Ok();
                }
            )
            .AllowAnonymous()
            .WithName("DeleteTag");
    }
}
