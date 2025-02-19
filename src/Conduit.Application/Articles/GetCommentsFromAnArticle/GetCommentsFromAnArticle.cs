using Conduit.Application.Abstractions.Cqrs;

namespace Conduit.Application.Articles.GetCommentsFromAnArticle;

public sealed record GetCommentsFromAnArticleCommand
(
    string CurrentUsersEmail,
    string Slug
) : ICommand<GetCommentsFromAnArticleCommandDto>;
