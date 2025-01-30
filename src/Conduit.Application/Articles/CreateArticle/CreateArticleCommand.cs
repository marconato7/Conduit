using Conduit.Application.Abstractions.Cqrs;

namespace Conduit.Application.Articles.CreateArticle;

public sealed record CreateArticleCommand
(
    string    CurrentUsersEmail,
    string    Title,
    string    Description,
    string    Body,
    string[]? TagList
) : ICommand<CreateArticleCommandDto>;
