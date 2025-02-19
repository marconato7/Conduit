using System.Security.Claims;
using Conduit.Api.Endpoints.Comments.AddCommentsToAnArticle;
using Conduit.Application.Articles.GetCommentsFromAnArticle;
using MediatR;

namespace Conduit.Api.Endpoints.Comments.GetCommentsFromAnArticle;

public class GetCommentsFromAnArticle : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapGet
            (
                "/api/articles/{slug}/comments",
                async
                (
                    string                        slug,
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

                    var getCommentsFromAnArticleCommand = new GetCommentsFromAnArticleCommand
                    (
                        CurrentUsersEmail: currentUsersEmail,
                        Slug:              slug
                    );

                    var getCommentsFromAnArticleCommandResult = await sender.Send
                    (
                        getCommentsFromAnArticleCommand,
                        cancellationToken
                    );

                    if (getCommentsFromAnArticleCommandResult.IsSuccess)
                    {
                        List<GetCommentsFromAnArticleResponseProps> getCommentsFromAnArticleResponsePropsList = [];

                        foreach (var comment in getCommentsFromAnArticleCommandResult.Value.Comments)
                        {
                            getCommentsFromAnArticleResponsePropsList.Add
                            (
                                new GetCommentsFromAnArticleResponseProps
                                (
                                    Id:        comment.Id,
                                    CreatedAt: comment.CreatedAt,
                                    UpdatedAt: comment.UpdatedAt,
                                    Body:      comment.Body,
                                    Author: new AuthorModelForResponse
                                    (
                                        Username:  comment.Author.Username,
                                        Bio:       comment.Author.Bio,
                                        Image:     comment.Author.Image,
                                        Following: comment.Author.Following
                                    )
                                )
                            );
                        }

                        var getCommentsFromAnArticleResponse = new GetCommentsFromAnArticleResponse
                        (
                            getCommentsFromAnArticleResponsePropsList
                        );

                        return Results.Ok(getCommentsFromAnArticleResponse);
                    }

                    return Results.InternalServerError();
                }
            )
            .AllowAnonymous();
    }
}
