using Conduit.Application.Abstractions.Cqrs;

namespace Conduit.Application.Articles.UnfavoriteArticle;

public sealed record UnfavoriteArticleCommand
(
    string CurrentUsersEmail,
    string Slug
) : ICommand<UnfavoriteArticleCommandDto>;
