using Conduit.Application.Abstractions.Cqrs;
using Conduit.Domain.Abstractions;
using Conduit.Domain.Articles;
using FluentResults;

namespace Conduit.Application.Articles.CreateArticle;

internal sealed class CreateArticleCommandHandler
(
    IArticleRepository articleRepository,
    IUnitOfWork        unitOfWork
) : ICommandHandler<CreateArticleCommand, CreateArticleCommandDto>
{
    private readonly IArticleRepository _articleRepository = articleRepository;
    private readonly IUnitOfWork        _unitOfWork        = unitOfWork;

    public async Task<Result<CreateArticleCommandDto>> Handle
    (
        CreateArticleCommand command,
        CancellationToken    cancellationToken
    )
    {
        var article = Article.Create
        (
            title:       command.Title,
            description: command.Description,
            body:        command.Body,
            tagList:     command.TagList
        );

        _articleRepository.Add(article);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateArticleCommandDto
        (
            Title:       article.Title,
            Description: article.Description,
            Body:        article.Body,
            TagList:     article.TagList
        );
    }
}
