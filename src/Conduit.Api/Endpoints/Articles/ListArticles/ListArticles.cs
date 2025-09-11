using System.Security.Claims;
using Conduit.Application.Articles.ListArticles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Api.Endpoints.Articles.ListArticles;

internal sealed class ListArticles : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapGet
            (
                "/api/articles",
                async
                (
                    [FromQuery] string? tag,
                    [FromQuery] string? author,
                    [FromQuery] string? favorited,
                    [FromQuery] int?    limit,
                    [FromQuery] int?    offset,
                    ISender             sender,
                    HttpContext         context,
                    CancellationToken   cancellationToken = default
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

                    var listArticlesQuery = new ListArticlesQuery
                    (
                        CurrentUsersEmail: currentUsersEmail,
                        Tag:               tag,
                        Author:            author,
                        Favorited:         favorited,
                        Limit:             limit,
                        Offset:            offset
                    );

                    var listArticlesQueryResult = await sender.Send
                    (
                        listArticlesQuery,
                        cancellationToken
                    );

                    if (listArticlesQueryResult.IsFailed)
                    {
                        return Results.BadRequest();
                    }

                    List<ListArticlesResponseProps> listArticlesResponsePropsList = [];

                    foreach (var article in listArticlesQueryResult.Value.Articles)
                    {
                        listArticlesResponsePropsList.Add
                        (
                            new ListArticlesResponseProps
                            (
                                Slug:           article.Slug,
                                Title:          article.Title,
                                Description:    article.Description,
                                TagList:        article.TagList,
                                CreatedAt:      article.CreatedAt,
                                UpdatedAt:      article.UpdatedAt,
                                Favorited:      article.Favorited,
                                FavoritesCount: article.FavoritesCount,
                                Author:         new AuthorModelForResponse
                                (
                                    Username:  article.Author.Username,
                                    Bio:       article.Author.Bio,
                                    Image:     article.Author.Image,
                                    Following: article.Author.Following
                                )
                            )
                        );
                    }

                    var listArticlesResponse = new ListArticlesResponse
                    (
                        listArticlesResponsePropsList,
                        listArticlesResponsePropsList.Count
                    );

                    return Results.Ok(listArticlesResponse);
                }
            )
            .AllowAnonymous();
    }
}
