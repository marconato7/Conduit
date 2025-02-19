using Conduit.Application.Abstractions.Cqrs;

namespace Conduit.Application.Articles.AddCommentsToAnArticle;

public sealed record AddCommentsToAnArticleCommand
(
    string CurrentUsersEmail,
    string Slug,
    string Body
) : ICommand<AddCommentsToAnArticleCommandDto>;
