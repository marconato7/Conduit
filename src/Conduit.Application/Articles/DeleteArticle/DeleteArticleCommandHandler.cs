using Conduit.Application.Abstractions.Cqrs;
using Conduit.Domain.Abstractions;
using Conduit.Domain.Articles;
using Conduit.Domain.Users;
using FluentResults;

namespace Conduit.Application.Articles.DeleteArticle;

internal sealed class DeleteArticleCommandHandler
(
    IArticleRepository articleRepository,
    IUnitOfWork        unitOfWork,
    IUserRepository    userRepository
) : ICommandHandler<DeleteArticleCommand>
{
    private readonly IArticleRepository _articleRepository = articleRepository;
    private readonly IUnitOfWork        _unitOfWork        = unitOfWork;
    private readonly IUserRepository    _userRepository    = userRepository;

    public async Task<Result> Handle
    (
        DeleteArticleCommand command,
        CancellationToken    cancellationToken
    )
    {
        var currentUser = await _userRepository.GetByEmailAsync(command.CurrentUsersEmail, cancellationToken);
        if (currentUser is null) return Result.Fail("something went wrong");

        var article = await _articleRepository.GetBySlugAsync(command.Slug, cancellationToken);
        if (article is null) return Result.Fail(ArticleErrors.NotFound.Message);

        if (article.AuthorId != currentUser.Id)
        {
            return Result.Fail("something went wrong");
        }

        currentUser.RemoveArticle(article);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
