using Conduit.Application.Articles.Shared;

namespace Conduit.Application.Articles.AddCommentsToAnArticle;

public sealed record AddCommentsToAnArticleCommandDto
(
    Guid        Id,
    DateTime    CreatedAt,
    DateTime    UpdatedAt,
    string      Body,
    AuthorModel Author
);
