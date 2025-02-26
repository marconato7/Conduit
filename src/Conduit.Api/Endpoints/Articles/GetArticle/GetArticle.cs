using System.Security.Claims;
using Conduit.Application.Articles.GetArticle;
using Conduit.Application.Articles.Shared;
using MediatR;

namespace Conduit.Api.Endpoints.Articles.GetArticle;

public class GetArticle : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapGet
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

                    var getArticleQuery = new GetArticleQuery
                    (
                        CurrentUsersEmail: currentUsersEmail,
                        Slug:              slug
                    );

                    var getArticleQueryResult = await sender.Send
                    (
                        getArticleQuery,
                        cancellationToken
                    );

                    if (getArticleQueryResult.IsSuccess)
                    {
                        var favoriteArticleResponse = new GetArticleResponse
                        (
                            Article: new GetArticleResponseProps
                            (
                                Slug:           getArticleQueryResult.Value.Slug,
                                Title:          getArticleQueryResult.Value.Title,
                                Description:    getArticleQueryResult.Value.Description,
                                Body:           getArticleQueryResult.Value.Body,
                                TagList:        getArticleQueryResult.Value.TagList,
                                CreatedAt:      getArticleQueryResult.Value.CreatedAt,
                                UpdatedAt:      getArticleQueryResult.Value.UpdatedAt,
                                Favorited:      getArticleQueryResult.Value.Favorited,
                                FavoritesCount: getArticleQueryResult.Value.FavoritesCount,
                                Author:         new AuthorModelForResponse
                                (
                                    Username:  getArticleQueryResult.Value.Author.Username,
                                    Bio:       getArticleQueryResult.Value.Author.Bio,
                                    Image:     getArticleQueryResult.Value.Author.Image,
                                    Following: getArticleQueryResult.Value.Author.Following
                                )
                            )
                        );

                        return Results.Ok(favoriteArticleResponse);
                    }
                    else
                    {
                        if
                        (
                            getArticleQueryResult.IsFailed &&
                            getArticleQueryResult.Errors.First().Message
                                == ArticlesErrors.NotFound.Message
                        )
                        {
                            return Results.NotFound();
                        }
                    }

                    return Results.InternalServerError();
                }
            )
            .AllowAnonymous();
    }
}
