using Conduit.Application.Abstractions.Cqrs;
using Conduit.Application.Articles.Shared;
using Conduit.Domain.Articles;
using Conduit.Domain.Users;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Conduit.Application.Articles.ListArticles;

internal sealed class ListArticlesQueryHandler
(
    IArticleRepository                articleRepository,
    ILogger<ListArticlesQueryHandler> logger,
    IUserRepository                   userRepository
)
    : ICommandHandler<ListArticlesQuery, ListArticlesQueryDto>
{
    private readonly IArticleRepository                _articleRepository = articleRepository;
    private readonly ILogger<ListArticlesQueryHandler> _logger            = logger;
    private readonly IUserRepository                   _userRepository    = userRepository;

    public async Task<Result<ListArticlesQueryDto>> Handle
    (
        ListArticlesQuery query,
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

        List<ListArticlesQueryDtoProps> listArticlesQueryDtoProps = [];

        var articles = await _articleRepository.ListArticles
        (
            tag:               query.Tag,
            author:            query.Author,
            favorited:         query.Favorited,
            limit:             query.Limit,
            offset:            query.Offset,
            cancellationToken: cancellationToken
        );

        foreach (var article in articles)
        {
            listArticlesQueryDtoProps.Add
            (
                new ListArticlesQueryDtoProps
                (
                    Slug:           article.Slug,
                    Title:          article.Title,
                    Description:    article.Description,
                    TagList:        article.Tags is null ? [] : query.Tag is null ? [.. article.Tags.Select(tag => tag.Name)] : [.. article.Tags.OrderBy(tag => tag.Name != query.Tag).Select(tag => tag.Name)],
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

        var listArticlesQueryDto = new ListArticlesQueryDto
        (
            listArticlesQueryDtoProps, articles.Count());

        return listArticlesQueryDto;
    }
}
