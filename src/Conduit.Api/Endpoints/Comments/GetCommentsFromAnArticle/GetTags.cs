using MediatR;

namespace Conduit.Api.Endpoints.Articles.GetTags;

public class GetTags : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        Console.WriteLine("GetTags.MapEndpoint");

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
                    await Task.Run(() => Console.WriteLine("GetTags"), cancellationToken);
                    return Task.CompletedTask;
                })
            .AllowAnonymous();
    }
}

// Returns a List of Tags

// List of Tags

// {
//     "tags": [
//         "reactjs",
//         "angularjs"
//     ]
// }
