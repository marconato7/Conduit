using Conduit.Application.Abstractions.Cqrs;

namespace Conduit.Application.Articles.CreateArticle;

public sealed record CreateArticleCommand
(
    string        CurrentUsersEmail,
    string        Title,
    string        Description,
    string        Body,
    List<string>? TagList
) : ICommand<CreateArticleCommandDto>;
