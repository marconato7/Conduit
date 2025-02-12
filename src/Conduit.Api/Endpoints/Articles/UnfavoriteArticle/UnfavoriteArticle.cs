using System.Security.Claims;
using Conduit.Application.Articles.UnfavoriteArticle;
using MediatR;

namespace Conduit.Api.Endpoints.Articles.UnfavoriteArticle;

public class UnfavoriteArticle : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        Console.WriteLine("GetTags.MapEndpoint");

        app
            .MapDelete
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
                    string stepOne = context.Request.Headers.Authorization!;
                    string[] stepTwo = stepOne.Split("Token ");
                    string token = stepTwo[1];

                    var currentUsersEmail = context.User.Claims.FirstOrDefault
                        (claim => claim.Type == ClaimTypes.Email)?.Value;

                    if (currentUsersEmail is null)
                    {
                        return Results.Unauthorized();
                    }

                    var favoriteArticleCommand = new UnfavoriteArticleCommand
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
                        var favoriteArticleResponse = new UnfavoriteArticleResponse
                        (
                            Article: new UnfavoriteArticleResponseProps
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
