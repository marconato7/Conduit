using Conduit.Application.Abstractions.Cqrs;

namespace Conduit.Application.Articles.DeleteComment;

public sealed record DeleteCommentCommand
(
    string CurrentUsersEmail,
    string Slug,
    Guid   CommentId
) : ICommand;
