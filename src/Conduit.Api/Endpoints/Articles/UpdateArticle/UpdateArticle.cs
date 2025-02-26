using System.Security.Claims;
using Conduit.Application.Articles.UpdateArticle;
using MediatR;

namespace Conduit.Api.Endpoints.Articles.UpdateArticle;

public class UpdateArticle : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapPut
            (
                "/api/articles/{slug}",
                async
                (
                    string               slug,
                    UpdateArticleRequest request,
                    ISender              sender,
                    HttpContext          context,
                    CancellationToken    cancellationToken = default
                ) =>
                {
                    var currentUsersEmail = context.User.Claims.FirstOrDefault
                        (claim => claim.Type == ClaimTypes.Email)?.Value;

                    if (currentUsersEmail is null)
                    {
                        return Results.Unauthorized();
                    }

                    var updateArticleCommand = new UpdateArticleCommand
                    (
                        CurrentUsersEmail: currentUsersEmail,
                        Slug:              slug,
                        Title:             request.Article.Title,
                        Description:       request.Article.Description,
                        Body:              request.Article.Body
                    );

                    var updateArticleCommandResult = await sender.Send
                    (
                        updateArticleCommand,
                        cancellationToken
                    );

                    if (updateArticleCommandResult.IsSuccess)
                    {
                        var updateArticleResponse = new UpdateArticleResponse
                        (
                            Article: new UpdateArticleResponseProps
                            (
                                Slug:           updateArticleCommandResult.Value.Slug,
                                Title:          updateArticleCommandResult.Value.Title,
                                Description:    updateArticleCommandResult.Value.Description,
                                Body:           updateArticleCommandResult.Value.Body,
                                TagList:        updateArticleCommandResult.Value.TagList,
                                CreatedAt:      updateArticleCommandResult.Value.CreatedAt,
                                UpdatedAt:      updateArticleCommandResult.Value.UpdatedAt,
                                Favorited:      updateArticleCommandResult.Value.Favorited,
                                FavoritesCount: updateArticleCommandResult.Value.FavoritesCount,
                                Author:         new AuthorModelForResponse
                                (
                                    Username:  updateArticleCommandResult.Value.Author.Username,
                                    Bio:       updateArticleCommandResult.Value.Author.Bio,
                                    Image:     updateArticleCommandResult.Value.Author.Image,
                                    Following: updateArticleCommandResult.Value.Author.Following
                                )
                            )
                        );

                        return Results.Ok(updateArticleResponse);
                    }

                    return Results.InternalServerError();
                }
            )
            .RequireAuthorization();
    }
}
