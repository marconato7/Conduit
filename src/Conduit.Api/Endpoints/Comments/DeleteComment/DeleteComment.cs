using System.Security.Claims;
using Conduit.Application.Articles.DeleteComment;
using MediatR;

namespace Conduit.Api.Endpoints.Comments.DeleteComment;

public class DeleteComment : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapDelete
            (
                "/api/articles/{slug}/comments/{commentId}",
                async
                (
                    string            slug,
                    Guid              commentId,
                    ISender           sender,
                    HttpContext       context,
                    CancellationToken cancellationToken = default
                ) =>
                {
                    var currentUsersEmail = context.User.Claims.FirstOrDefault
                        (claim => claim.Type == ClaimTypes.Email)?.Value;

                    if (currentUsersEmail is null)
                    {
                        return Results.Unauthorized();
                    }

                    var deleteCommentCommand = new DeleteCommentCommand
                    (
                        CurrentUsersEmail: currentUsersEmail,
                        Slug:              slug,
                        CommentId:         commentId
                    );

                    var deleteCommentCommandResult = await sender.Send
                    (
                        deleteCommentCommand,
                        cancellationToken
                    );

                    if (deleteCommentCommandResult.IsSuccess)
                    {
                        return Results.NoContent();
                    }

                    return Results.InternalServerError();
                }
            )
            .RequireAuthorization();
    }
}
