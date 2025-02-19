using Conduit.Application.Abstractions.Clock;
using Conduit.Application.Abstractions.Cqrs;
using Conduit.Application.Articles.Shared;
using Conduit.Domain.Abstractions;
using Conduit.Domain.Articles;
using Conduit.Domain.Users;
using FluentResults;

namespace Conduit.Application.Articles.AddCommentsToAnArticle;

internal sealed class AddCommentsToAnArticleCommandHandler
(
    IArticleRepository articleRepository,
    IDateTimeProvider  dateTimeProvider,
    IUnitOfWork        unitOfWork,
    IUserRepository    userRepository
) : ICommandHandler<AddCommentsToAnArticleCommand, AddCommentsToAnArticleCommandDto>
{
    private readonly IArticleRepository _articleRepository = articleRepository;
    private readonly IDateTimeProvider  _dateTimeProvider  = dateTimeProvider;
    private readonly IUnitOfWork        _unitOfWork        = unitOfWork;
    private readonly IUserRepository    _userRepository    = userRepository;

    public async Task<Result<AddCommentsToAnArticleCommandDto>> Handle
    (
        AddCommentsToAnArticleCommand command,
        CancellationToken             cancellationToken
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
            command.Slug,
            cancellationToken
        );

        if (article is null)
        {
            return Result.Fail("something went wrong");
        }

        var comment = Comment.Create
        (
            command.Body,
            currentUser,
            _dateTimeProvider.UtcNow
        );

        article.AddComment(comment);

        await _articleRepository.StoreArticleWithComment
        (
            article:           article,
            comment:           comment,
            cancellationToken: cancellationToken
        );

        var addCommentsToAnArticleCommandDto = new AddCommentsToAnArticleCommandDto
        (
            Id:        comment.Id,
            CreatedAt: article.CreatedAtUtc,
            UpdatedAt: article.CreatedAtUtc,
            Body:      article.Body,
            Author:    new AuthorModel
            (
                Username:  currentUser.Username,
                Bio:       currentUser.Bio,
                Image:     currentUser.Image,
                Following: false
            )
        );

        return addCommentsToAnArticleCommandDto;
    }
}
