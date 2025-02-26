using Conduit.Application.Abstractions.Cqrs;
using Conduit.Application.Articles.ListArticles;
using Conduit.Application.Articles.Shared;
using Conduit.Domain.Articles;
using Conduit.Domain.Users;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Conduit.Application.Articles.FeedArticles;

internal sealed class FeedArticlesQueryHandler
(
    IArticleRepository                articleRepository,
    ILogger<ListArticlesQueryHandler> logger,
    IUserRepository                   userRepository
)
    : ICommandHandler<FeedArticlesQuery, FeedArticlesQueryDto>
{
    private readonly IArticleRepository                _articleRepository = articleRepository;
    private readonly ILogger<ListArticlesQueryHandler> _logger            = logger;
    private readonly IUserRepository                   _userRepository    = userRepository;

    public async Task<Result<FeedArticlesQueryDto>> Handle
    (
        FeedArticlesQuery query,
        CancellationToken cancellationToken
    )
    {
        User? currentUser = null;

        if (query.CurrentUsersEmail is not null)
        {
            currentUser = await _userRepository.GetByEmailAsync
            (
                email:             query.CurrentUsersEmail,
                cancellationToken: cancellationToken
            );
        }

        if (currentUser is null)
        {
            return Result.Fail("user not found");
        }

        var articles = await _articleRepository.FeedArticles
        (
            user:              currentUser,
            limit:             query.Limit,
            offset:            query.Offset,
            cancellationToken: cancellationToken
        );

        if (articles is null)
        {
            // return empty response
            return Result.Fail("");
        }

        List<FeedArticlesQueryDtoProps> feedArticlesQueryDtoProps = [];

        foreach (var article in articles)
        {
            feedArticlesQueryDtoProps.Add
            (
                new FeedArticlesQueryDtoProps
                (
                    Slug:           article.Slug,
                    Title:          article.Title,
                    Description:    article.Description,
                    TagList:        article.Tags is null ? [] : [.. article.Tags.Select(tag => tag.Name)],
                    CreatedAt:      article.CreatedAtUtc,
                    UpdatedAt:      article.UpdatedAtUtc,
                    Favorited:      currentUser is not null && currentUser.FavoriteArticles.Any(a => a.Id == article.Id),
                    FavoritesCount: article.UsersThatFavorited.Count,
                    Author:         new AuthorModel
                    (
                        Username:  article.Author.Username,
                        Bio:       article.Author.Bio,
                        Image:     article.Author.Image,
                        Following: currentUser is not null && currentUser.Following.Any(u => u.Id == article.Author.Id)
                    )
                )
            );
        }

        var feedArticlesQueryDto = new FeedArticlesQueryDto
        (
            feedArticlesQueryDtoProps,
            feedArticlesQueryDtoProps.Count
        );

        return feedArticlesQueryDto;
    }
}
