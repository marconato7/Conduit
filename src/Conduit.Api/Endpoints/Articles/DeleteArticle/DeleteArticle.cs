using System.Security.Claims;
using Conduit.Application.Articles;
using Conduit.Application.Articles.DeleteArticle;
using Conduit.Application.Articles.GetArticle;
using Conduit.Application.Articles.Shared;
using MediatR;

namespace Conduit.Api.Endpoints.Articles.DeleteArticle;

public class DeleteArticle : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapDelete
            (
                "/api/articles/{slug}",
                async
                (
                    string            slug,
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

                    var deleteArticleCommand = new DeleteArticleCommand
                    (
                        CurrentUsersEmail: currentUsersEmail,
                        Slug:              slug
                    );

                    var deleteArticleCommandResult = await sender.Send
                    (
                        deleteArticleCommand,
                        cancellationToken
                    );

                    if (deleteArticleCommandResult.IsSuccess)
                    {
                        return Results.Ok();
                    }
                    else
                    {
                        if
                        (
                            deleteArticleCommandResult.IsFailed &&
                            deleteArticleCommandResult.Errors.First().Message
                                == ArticlesErrors.NotFound.Message)
                        {
                            return Results.NotFound();
                        }
                    }

                    return Results.InternalServerError();
                }
            )
            .RequireAuthorization();
    }
}
