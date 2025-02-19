using Conduit.Application.Articles.GetTags;
using MediatR;

namespace Conduit.Api.Endpoints.Tags.GetTags;

public class GetTags : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapGet
            (
                "/api/tags",
                async
                (
                    ISender sender,
                    CancellationToken cancellationToken
                ) =>
                {
                    var getTagsQuery = new GetTagsQuery();
                    
                    var getTagsQueryResult = await sender.Send
                    (
                        getTagsQuery,
                        cancellationToken
                    );

                    if (getTagsQueryResult.IsSuccess)
                    {
                        var getTagsQueryResponse = new GetTagsQueryResponse(getTagsQueryResult.Value.Tags);

                        return Results.Ok(getTagsQueryResponse);
                    }

                    return Results.InternalServerError();
                }
            )
            .AllowAnonymous();
    }
}
