using Conduit.Api.Data;
using Conduit.Api.Features.GetTagById;
using Conduit.Api.Models;

namespace Conduit.Api.Features.CreateTag;

public static partial class CreateTag
{
    public static void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder
            .MapPost
            (
                "api/tags",
                async (
                    CreateTagRequest request,
                    ApplicationDbContext applicationDbContext,
                    CancellationToken cancellationToken
                ) =>
                {
                    var tag = new Tag(request.Name);

                    applicationDbContext.Add(tag);

                    await applicationDbContext.SaveChangesAsync(
                        cancellationToken
                    );

                    var tagDto = new TagDto(tag.Id, tag.Name);

                    return Results.CreatedAtRoute(
                        "GetTagById", new { tag.Id }, tagDto
                    );
                }
            )
            .AllowAnonymous()
            .WithName("CreateTag");
    }
}
