using Conduit.Application.Abstractions.Cqrs;

namespace Conduit.Application.Articles.UpdateArticle;

public sealed record UpdateArticleCommand
(
    string  CurrentUsersEmail,
    string  Slug,
    string? Title,
    string? Description,
    string? Body
) : ICommand<UpdateArticleCommandDto>;
