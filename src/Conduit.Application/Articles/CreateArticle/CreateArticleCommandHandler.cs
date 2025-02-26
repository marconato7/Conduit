using Conduit.Application.Abstractions.Clock;
using Conduit.Application.Abstractions.Cqrs;
using Conduit.Application.Articles.Shared;
using Conduit.Domain.Abstractions;
using Conduit.Domain.Articles;
using Conduit.Domain.Users;
using FluentResults;

namespace Conduit.Application.Articles.CreateArticle;

internal sealed class CreateArticleCommandHandler
(
    IArticleRepository articleRepository,
    IDateTimeProvider  dateTimeProvider,
    IUnitOfWork        unitOfWork,
    IUserRepository    userRepository
) : ICommandHandler<CreateArticleCommand, CreateArticleCommandDto>
{
    private readonly IArticleRepository _articleRepository = articleRepository;
    private readonly IDateTimeProvider  _dateTimeProvider  = dateTimeProvider;
    private readonly IUnitOfWork        _unitOfWork        = unitOfWork;
    private readonly IUserRepository    _userRepository    = userRepository;

    public async Task<Result<CreateArticleCommandDto>> Handle
    (
        CreateArticleCommand command,
        CancellationToken    cancellationToken
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

        List<Tag>? existingTags               = [];
        List<string>? tagsThatNeedToBeCreated = [];

        if (command.TagList is not null)
        {
            existingTags = await _articleRepository.GetTagsByName
            (
                tagNames:          command.TagList,
                cancellationToken: cancellationToken
            );

            foreach (var tagName in command.TagList)
            {
                if (existingTags is not null)
                {
                    var check = existingTags.Any(existingTag => existingTag.Name == tagName);
                    if (!check)
                    {
                        tagsThatNeedToBeCreated.Add(tagName);
                    }
                }
            }
        }

        var article = Article.Create
        (
            title:                   command.Title,
            description:             command.Description,
            body:                    command.Body,
            author:                  currentUser,
            createdAtUtc:            _dateTimeProvider.UtcNow,
            tagsThatNeedToBeCreated: tagsThatNeedToBeCreated,
            existingTags:            existingTags
        );

        _articleRepository.Add(article);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateArticleCommandDto
        (
            Slug:           article.Slug,
            Title:          article.Title,
            Description:    article.Description,
            Body:           article.Body,
            TagList:        [.. article.Tags.Select(t => t.Name)],
            CreatedAt:      article.CreatedAtUtc,
            UpdatedAt:      article.CreatedAtUtc,
            Favorited:      false,
            FavoritesCount: 0,
            Author:         new AuthorModel
            (
                Username:  currentUser.Username,
                Bio:       currentUser.Bio,
                Image:     currentUser.Image,
                Following: false
            )
        );
    }
}
