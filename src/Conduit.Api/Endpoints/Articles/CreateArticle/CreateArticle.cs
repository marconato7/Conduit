using System.Security.Claims;
using Conduit.Application.Articles.CreateArticle;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Api.Endpoints.Articles.CreateArticle;

public class CreateArticle : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapPost
            (
                "/api/articles",
                async
                (
                    [FromBody] CreateArticleRequest  request,
                    ISender                          sender,
                    HttpContext                      context,
                    CancellationToken                cancellationToken = default
                ) =>
                {
                    var currentUsersEmail = context
                        .User
                        .Claims
                        .FirstOrDefault
                        (
                            claim => claim.Type == ClaimTypes.Email
                        )
                        ?.Value;

                    if (currentUsersEmail is null)
                    {
                        return Results.Unauthorized();
                    }

                    var createArticleCommand = new CreateArticleCommand
                    (
                        CurrentUsersEmail: currentUsersEmail,
                        Title:             request.Article.Title,
                        Description:       request.Article.Description,
                        Body:              request.Article.Body,
                        TagList:           request.Article.TagList
                    );

                    var createArticleCommandResult = await sender.Send
                    (
                        createArticleCommand,
                        cancellationToken
                    );

                    if (createArticleCommandResult.IsFailed)
                    {
                        return Results.BadRequest();
                    }

                    var createArticleResponse = new CreateArticleResponse
                    (
                        new CreateArticleResponseProps
                        (
                            Slug:           createArticleCommandResult.Value.Slug,
                            Title:          createArticleCommandResult.Value.Title,
                            Description:    createArticleCommandResult.Value.Description,
                            Body:           createArticleCommandResult.Value.Body,
                            TagList:        createArticleCommandResult.Value.TagList,
                            CreatedAt:      createArticleCommandResult.Value.CreatedAt,
                            UpdatedAt:      createArticleCommandResult.Value.UpdatedAt,
                            Favorited:      createArticleCommandResult.Value.Favorited,
                            FavoritesCount: createArticleCommandResult.Value.FavoritesCount,
                            Author:         new AuthorModelForResponse
                            (
                                Username:  createArticleCommandResult.Value.Author.Username,
                                Bio:       createArticleCommandResult.Value.Author.Bio,
                                Image:     createArticleCommandResult.Value.Author.Image,
                                Following: createArticleCommandResult.Value.Author.Following
                            )
                        )
                    );

                    return Results.Ok(createArticleResponse);
                }
            )
            .RequireAuthorization();
    }
}
