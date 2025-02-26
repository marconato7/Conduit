using System.Security.Claims;
using Conduit.Application.Articles.FeedArticles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Api.Endpoints.Articles.FeedArticles;

public class FeedArticles : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapGet
            (
                "/api/articles/feed",
                async
                (
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

                    var feedArticlesQuery = new FeedArticlesQuery
                    (
                        CurrentUsersEmail: currentUsersEmail,
                        Limit:             limit,
                        Offset:            offset
                    );

                    var feedArticlesQueryResult = await sender.Send
                    (
                        feedArticlesQuery,
                        cancellationToken
                    );

                    if (feedArticlesQueryResult.IsFailed)
                    {
                        return Results.BadRequest();
                    }

                    List<FeedArticlesResponseProps> feedArticlesResponsePropsList = [];

                    foreach (var article in feedArticlesQueryResult.Value.Articles)
                    {
                        feedArticlesResponsePropsList.Add
                        (
                            new FeedArticlesResponseProps
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

                    var listArticlesResponse = new FeedArticlesResponse
                    (
                        feedArticlesResponsePropsList,
                        feedArticlesResponsePropsList.Count
                    );

                    return Results.Ok(listArticlesResponse);
                }
            )
            .RequireAuthorization();
    }
}
