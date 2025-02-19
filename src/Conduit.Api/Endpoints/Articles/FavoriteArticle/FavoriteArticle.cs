using System.Security.Claims;
using Conduit.Application.Articles.FavoriteArticle;
using MediatR;

namespace Conduit.Api.Endpoints.Articles.FavoriteArticle;

public class FavoriteArticle : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
            .MapPost
            (
                "/api/articles/{slug}/favorite",
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

                    var favoriteArticleCommand = new FavoriteArticleCommand
                    (
                        CurrentUsersEmail: currentUsersEmail,
                        Slug:              slug
                    );

                    var favoriteArticleCommandResult = await sender.Send
                    (
                        favoriteArticleCommand,
                        cancellationToken
                    );

                    if (favoriteArticleCommandResult.IsSuccess)
                    {
                        var favoriteArticleResponse = new FavoriteArticleResponse
                        (
                            Article: new FavoriteArticleResponseProps
                            (
                                Slug:           favoriteArticleCommandResult.Value.Slug,
                                Title:          favoriteArticleCommandResult.Value.Title,
                                Description:    favoriteArticleCommandResult.Value.Description,
                                Body:           favoriteArticleCommandResult.Value.Body,
                                TagList:        favoriteArticleCommandResult.Value.TagList,
                                CreatedAt:      favoriteArticleCommandResult.Value.CreatedAt,
                                UpdatedAt:      favoriteArticleCommandResult.Value.UpdatedAt,
                                Favorited:      favoriteArticleCommandResult.Value.Favorited,
                                FavoritesCount: favoriteArticleCommandResult.Value.FavoritesCount,
                                Author:         new AuthorModelForResponse
                                (
                                    Username:  favoriteArticleCommandResult.Value.Author.Username,
                                    Bio:       favoriteArticleCommandResult.Value.Author.Bio,
                                    Image:     favoriteArticleCommandResult.Value.Author.Image,
                                    Following: favoriteArticleCommandResult.Value.Author.Following
                                )
                            )
                        );

                        return Results.Ok(favoriteArticleResponse);
                    }

                    return Results.Unauthorized();
                }
            )
            .RequireAuthorization();
    }
}
