using Conduit.Application.Abstractions.Cqrs;
using Conduit.Application.Articles.Shared;
using Conduit.Domain.Abstractions;
using Conduit.Domain.Articles;
using Conduit.Domain.Tags;
using Conduit.Domain.Users;
using FluentResults;

namespace Conduit.Application.Articles.UnfavoriteArticle;

internal sealed class UnfavoriteArticleCommandHandler
(
    IArticleRepository articleRepository,
    IUnitOfWork        unitOfWork,
    IUserRepository    userRepository
) : ICommandHandler<UnfavoriteArticleCommand, UnfavoriteArticleCommandDto>
{
    private readonly IArticleRepository _articleRepository = articleRepository;
    private readonly IUnitOfWork        _unitOfWork        = unitOfWork;
    private readonly IUserRepository    _userRepository    = userRepository;

    public async Task<Result<UnfavoriteArticleCommandDto>> Handle
    (
        UnfavoriteArticleCommand command,
        CancellationToken        cancellationToken
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

        currentUser.UnfavoriteArticle(article);

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

        var unfavoriteArticleCommandDto = new UnfavoriteArticleCommandDto
        (
            Slug:           article.Slug,
            Title:          article.Title,
            Description:    article.Description,
            Body:           article.Body,
            TagList:        TagListToStringList(article.TagList),
            CreatedAt:      article.CreatedAtUtc,
            UpdatedAt:      article.UpdatedAtUtc,
            Favorited:      false,
            FavoritesCount: article.UsersThatFavorited.Count,
            Author:         new AuthorModel
            (
                Username:  articlesAuthor.Username,
                Bio:       articlesAuthor.Bio,
                Image:     articlesAuthor.Image,
                Following: currentUser.IsFollowing(articlesAuthor)
            )
        );

        return Result.Ok(unfavoriteArticleCommandDto);

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
