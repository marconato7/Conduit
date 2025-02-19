using System.Security.Claims;
using Conduit.Application.Articles.AddCommentsToAnArticle;
using MediatR;

namespace Conduit.Api.Endpoints.Comments.AddCommentsToAnArticle;

public class AddCommentsToAnArticle : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapPost
            (
                "/api/articles/{slug}/comments",
                async
                (
                    string                        slug,
                    AddCommentsToAnArticleRequest request,
                    ISender                       sender,
                    HttpContext                   context,
                    CancellationToken             cancellationToken = default
                ) =>
                {
                    var currentUsersEmail = context.User.Claims.FirstOrDefault
                        (claim => claim.Type == ClaimTypes.Email)?.Value;

                    if (currentUsersEmail is null)
                    {
                        return Results.Unauthorized();
                    }

                    var addCommentsToAnArticleCommand = new AddCommentsToAnArticleCommand
                    (
                        CurrentUsersEmail: currentUsersEmail,
                        Slug:              slug,
                        Body:              request.Comment.Body
                    );

                    var addCommentsToAnArticleCommandResult = await sender.Send
                    (
                        addCommentsToAnArticleCommand,
                        cancellationToken
                    );

                    if (addCommentsToAnArticleCommandResult.IsSuccess)
                    {
                        var addCommentsToAnArticleResponse = new AddCommentsToAnArticleResponse
                        (
                            Comment: new AddCommentsToAnArticleResponseProps
                            (
                                Id:        addCommentsToAnArticleCommandResult.Value.Id,
                                CreatedAt: addCommentsToAnArticleCommandResult.Value.CreatedAt,
                                UpdatedAt: addCommentsToAnArticleCommandResult.Value.UpdatedAt,
                                Body:      addCommentsToAnArticleCommandResult.Value.Body,
                                Author:    new AuthorModelForResponse
                                (
                                    Username:  addCommentsToAnArticleCommandResult.Value.Author.Username,
                                    Bio:       addCommentsToAnArticleCommandResult.Value.Author.Bio,
                                    Image:     addCommentsToAnArticleCommandResult.Value.Author.Image,
                                    Following: addCommentsToAnArticleCommandResult.Value.Author.Following
                                )
                            )
                        );

                        return Results.Ok(addCommentsToAnArticleResponse);
                    }

                    return Results.InternalServerError();
                }
            )
            .RequireAuthorization();
    }
}
