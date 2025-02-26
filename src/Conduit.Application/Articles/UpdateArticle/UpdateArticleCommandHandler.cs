using Conduit.Application.Abstractions.Cqrs;
using Conduit.Application.Articles.Shared;
using Conduit.Domain.Abstractions;
using Conduit.Domain.Articles;
using Conduit.Domain.Users;
using FluentResults;

namespace Conduit.Application.Articles.UpdateArticle;

internal sealed class UpdateArticleCommandHandler
(
    IArticleRepository articleRepository,
    IUnitOfWork        unitOfWork,
    IUserRepository    userRepository
) : ICommandHandler<UpdateArticleCommand, UpdateArticleCommandDto>
{
    private readonly IArticleRepository _articleRepository = articleRepository;
    private readonly IUnitOfWork        _unitOfWork        = unitOfWork;
    private readonly IUserRepository    _userRepository    = userRepository;

    public async Task<Result<UpdateArticleCommandDto>> Handle
    (
        UpdateArticleCommand command,
        CancellationToken        cancellationToken
    )
    {
        var article = await _articleRepository.GetBySlugAsync
        (
            slug:              command.Slug,
            cancellationToken: cancellationToken
        );

        if (article is null)
        {
            return Result.Fail("something went wrong");
        }

        var currentUser = await _userRepository.GetByEmailAsync
        (
            email:             command.CurrentUsersEmail,
            cancellationToken: cancellationToken
        );

        if (currentUser is null || !Equals(currentUser.Id, article.AuthorId))
        {
            return Result.Fail("something went wrong");
        }

        article.Update
        (
            title:       command.Title,
            description: command.Description,
            body:        command.Body
        );

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var updateArticleCommandDto = new UpdateArticleCommandDto
        (
            Slug:           article.Slug,
            Title:          article.Title,
            Description:    article.Description,
            Body:           article.Body,
            TagList:        TagListToStringList(article.Tags),
            CreatedAt:      article.CreatedAtUtc,
            UpdatedAt:      article.UpdatedAtUtc,
            Favorited:      false,
            FavoritesCount: article.UsersThatFavorited.Count,
            Author:         new AuthorModel
            (
                Username:  currentUser.Username,
                Bio:       currentUser.Bio,
                Image:     currentUser.Image,
                Following: false
            )
        );

        return Result.Ok(updateArticleCommandDto);

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
