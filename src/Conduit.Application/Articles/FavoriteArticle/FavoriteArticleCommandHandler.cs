using Conduit.Application.Abstractions.Cqrs;
using Conduit.Application.Articles.Shared;
using Conduit.Domain.Abstractions;
using Conduit.Domain.Articles;
using Conduit.Domain.Users;
using FluentResults;

namespace Conduit.Application.Articles.FavoriteArticle;

internal sealed class FavoriteArticleCommandHandler
(
    IArticleRepository articleRepository,
    IUnitOfWork        unitOfWork,
    IUserRepository    userRepository
) : ICommandHandler<FavoriteArticleCommand, FavoriteArticleCommandDto>
{
    private readonly IArticleRepository _articleRepository = articleRepository;
    private readonly IUnitOfWork        _unitOfWork        = unitOfWork;
    private readonly IUserRepository    _userRepository    = userRepository;

    public async Task<Result<FavoriteArticleCommandDto>> Handle
    (
        FavoriteArticleCommand command,
        CancellationToken cancellationToken
    )
    {
        var currentUser = await _userRepository.GetByEmailAsync
        (
            email:             command.CurrentUsersEmail,
            cancellationToken: cancellationToken
        );

        if (currentUser is null)
        {
            return Result.Fail("something went wrong");
        }

        var article = await _articleRepository.GetBySlugAsync
        (
            slug:              command.Slug,
            cancellationToken: cancellationToken
        );

        if (article is null)
        {
            return Result.Fail("something went wrong");
        }

        currentUser.FavoriteArticle(article);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var articlesAuthor = await _userRepository.GetByEmailAsync
        (
            email:             article.Author.Email,
            cancellationToken: cancellationToken
        );

        if (articlesAuthor is null)
        {
            return Result.Fail("something went wrong");
        }

        var favoriteArticleCommandDto = new FavoriteArticleCommandDto
        (
            Slug:           article.Slug,
            Title:          article.Title,
            Description:    article.Description,
            Body:           article.Body,
            TagList:        TagListToStringList(article.TagList),
            CreatedAt:      article.CreatedAtUtc,
            UpdatedAt:      article.UpdatedAtUtc,
            Favorited:      true,
            FavoritesCount: article.UsersThatFavorited.Count,
            Author:         new AuthorModel
            (
                Username:  articlesAuthor.Username,
                Bio:       articlesAuthor.Bio,
                Image:     articlesAuthor.Image,
                Following: currentUser.IsFollowing(articlesAuthor)
            )
        );

        return Result.Ok(favoriteArticleCommandDto);

        static List<string> TagListToStringList(List<Tag> tags)
        {
            if (tags is null)
            {
                return [];
            }

            return [.. tags.Select(tag => tag.Name)];
        }
    }
}
