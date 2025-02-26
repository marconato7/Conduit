using Conduit.Application.Abstractions.Cqrs;
using Conduit.Application.Articles.Shared;
using Conduit.Domain.Articles;
using Conduit.Domain.Users;
using FluentResults;

namespace Conduit.Application.Articles.GetCommentsFromAnArticle;

internal sealed class GetCommentsFromAnArticleCommandHandler
(
    IArticleRepository articleRepository,
    IUserRepository    userRepository
) : ICommandHandler<GetCommentsFromAnArticleCommand, GetCommentsFromAnArticleCommandDto>
{
    private readonly IArticleRepository _articleRepository = articleRepository;
    private readonly IUserRepository    _userRepository    = userRepository;

    public async Task<Result<GetCommentsFromAnArticleCommandDto>> Handle
    (
        GetCommentsFromAnArticleCommand command,
        CancellationToken               cancellationToken
    )
    {
        User? currentUser = null;

        currentUser = await _userRepository.GetByEmailAsync
        (
            email:             command.CurrentUsersEmail,
            cancellationToken: cancellationToken
        );

        var articleWithComments = await _articleRepository.GetBySlugWithCommentsAsync
        (
            slug:              command.Slug,
            cancellationToken: cancellationToken
        );

        if (articleWithComments is null)
        {
            return Result.Fail("something went wrong");
        }

        List<GetCommentsFromAnArticleCommandDtoProps> getCommentsFromAnArticleCommandDtoPropsList = [];

        foreach (var comment in articleWithComments.Comments)
        {
            getCommentsFromAnArticleCommandDtoPropsList.Add
            (
                new GetCommentsFromAnArticleCommandDtoProps
                (
                    Id:        comment.Id,
                    CreatedAt: comment.CreatedAtUtc,
                    UpdatedAt: comment.UpdatedAtUtc,
                    Body:      comment.Body,
                    Author: new AuthorModel
                    (
                        Username:  articleWithComments.Author.Username,
                        Bio:       articleWithComments.Author.Bio,
                        Image:     articleWithComments.Author.Image,
                        Following: currentUser is null ? false : currentUser.IsFollowing(articleWithComments.Author)
                    )
                )
            );
        }

        var getCommentsFromAnArticleCommandDto = new GetCommentsFromAnArticleCommandDto
        (
            getCommentsFromAnArticleCommandDtoPropsList
        );

        return getCommentsFromAnArticleCommandDto;
    }
}
