using Conduit.Application.Abstractions.Cqrs;

namespace Conduit.Application.Articles.DeleteArticle;

public sealed record DeleteArticleCommand
(
    string CurrentUsersEmail,
    string Slug
)
    : ICommand;
