using Conduit.Application.Abstractions.Cqrs;
using Conduit.Application.Articles.Shared;
using Conduit.Domain.Articles;
using Conduit.Domain.Users;
using FluentResults;

namespace Conduit.Application.Articles.GetArticle;

internal sealed class GetArticleQueryHandler
(
    IArticleRepository articleRepository,
    IUserRepository    userRepository
) : IQueryHandler<GetArticleQuery, GetArticleQueryDto>
{
    private readonly IArticleRepository _articleRepository = articleRepository;
    private readonly IUserRepository    _userRepository    = userRepository;

    public async Task<Result<GetArticleQueryDto>> Handle
    (
        GetArticleQuery   query,
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

        var article = await _articleRepository.GetArticleQueryAsync
        (
            query.Slug,
            cancellationToken
        );

        if (article is null)
        {
            return Result.Fail(ArticleErrors.NotFound.Message);
        }

        var getArticleQueryDto = new GetArticleQueryDto
        (
            Slug:           article.Slug,
            Title:          article.Title,
            Description:    article.Description,
            Body:           article.Body,
            TagList:        article.TagList is null ? [] : [.. article.TagList.Select(tag => tag.Name)],
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
        );

        return getArticleQueryDto;
    }
}
