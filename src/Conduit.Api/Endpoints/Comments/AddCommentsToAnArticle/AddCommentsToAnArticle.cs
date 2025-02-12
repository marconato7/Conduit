using System.Security.Claims;
using Conduit.Application.Articles.AddCommentsToAnArticle;
using MediatR;

namespace Conduit.Api.Endpoints.Comments.AddCommentsToAnArticle;

public class AddCommentsToAnArticle : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        Console.WriteLine("GetTags.MapEndpoint");

        app
            .MapPost
            (
                "/api/articles/{slug}/comments",
                async
                (
                    string            slug,
                    ISender           sender,
                    HttpContext       context,
                    CancellationToken cancellationToken = default
                ) =>
                {
                    string stepOne = context.Request.Headers.Authorization!;
                    string[] stepTwo = stepOne.Split("Token ");
                    string token = stepTwo[1];

                    var currentUsersEmail = context.User.Claims.FirstOrDefault
                        (claim => claim.Type == ClaimTypes.Email)?.Value;

                    if (currentUsersEmail is null)
                    {
                        return Results.Unauthorized();
                    }

                    var addCommentsToAnArticleCommand = new AddCommentsToAnArticleCommand
                    (
                        CurrentUsersEmail: currentUsersEmail,
                        Slug:              slug
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
                            Article: new AddCommentsToAnArticleResponseProps
                            (
                                Slug:           addCommentsToAnArticleCommandResult.Value.Slug,
                                Title:          addCommentsToAnArticleCommandResult.Value.Title,
                                Description:    addCommentsToAnArticleCommandResult.Value.Description,
                                Body:           addCommentsToAnArticleCommandResult.Value.Body,
                                TagList:        addCommentsToAnArticleCommandResult.Value.TagList,
                                CreatedAt:      addCommentsToAnArticleCommandResult.Value.CreatedAt,
                                UpdatedAt:      addCommentsToAnArticleCommandResult.Value.UpdatedAt,
                                Favorited:      addCommentsToAnArticleCommandResult.Value.Favorited,
                                FavoritesCount: addCommentsToAnArticleCommandResult.Value.FavoritesCount,
                                Author:         new AuthorModelForResponse
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

                    return Results.Unauthorized();
                }
            )
            .RequireAuthorization();
    }
}
