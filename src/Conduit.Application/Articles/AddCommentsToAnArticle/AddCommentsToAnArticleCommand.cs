using Conduit.Application.Abstractions.Cqrs;

namespace Conduit.Application.Articles.AddCommentsToAnArticle;

public sealed record AddCommentsToAnArticleCommand
(
    string        CurrentUsersEmail,
    string        Title,
    string        Description,
    string        Body,
    List<string>? TagList
) : ICommand<AddCommentsToAnArticleCommandDto>;
