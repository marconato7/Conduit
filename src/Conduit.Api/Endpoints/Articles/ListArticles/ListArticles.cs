using System.Security.Claims;
using Conduit.Api.Controllers.Articles.CreateArticle;
using Conduit.Application.Articles.CreateArticle;
using Conduit.Application.Articles.ListArticles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Api.Endpoints.Articles.ListArticles;

public class ListArticles : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapGet
            (
                "/api/articles",
                async
                (
                    [FromQuery] string tag,
                    [FromQuery] string author,
                    [FromQuery] string favorited,
                    [FromQuery] int    limit,
                    [FromQuery] int    offset,
                    ISender            sender,
                    HttpContext        context,
                    CancellationToken  cancellationToken = default
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

                    var createArticleCommand = new ListArticlesQuery
                    (
                        CurrentUsersEmail: currentUsersEmail,
                        Tag:               tag,
                        Author:            author,
                        Favorited:         favorited,
                        Limit:             limit,
                        Offset:            offset
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
            .AllowAnonymous();
    }
}
