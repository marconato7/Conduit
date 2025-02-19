using Conduit.Application.Abstractions.Clock;
using Conduit.Application.Abstractions.Cqrs;
using Conduit.Domain.Abstractions;
using Conduit.Domain.Articles;
using Conduit.Domain.Users;
using FluentResults;

namespace Conduit.Application.Articles.DeleteComment;

internal sealed class DeleteCommentCommandHandler
(
    IArticleRepository articleRepository,
    IDateTimeProvider  dateTimeProvider,
    IUnitOfWork        unitOfWork,
    IUserRepository    userRepository
) : ICommandHandler<DeleteCommentCommand>
{
    private readonly IArticleRepository _articleRepository = articleRepository;
    private readonly IDateTimeProvider  _dateTimeProvider  = dateTimeProvider;
    private readonly IUnitOfWork        _unitOfWork        = unitOfWork;
    private readonly IUserRepository    _userRepository    = userRepository;

    public async Task<Result> Handle
    (
        DeleteCommentCommand command,
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

        var article = await _articleRepository.GetBySlugAsync
        (
            slug:              command.Slug,
            cancellationToken: cancellationToken
        );

        if (article is null)
        {
            return Result.Fail("something went wrong");
        }

        article.RemoveComment(command.CommentId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
