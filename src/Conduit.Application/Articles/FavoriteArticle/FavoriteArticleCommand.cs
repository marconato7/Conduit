using Conduit.Application.Abstractions.Cqrs;

namespace Conduit.Application.Articles.FavoriteArticle;

public sealed record FavoriteArticleCommand
(
    string CurrentUsersEmail,
    string Slug
) : ICommand<FavoriteArticleCommandDto>;
