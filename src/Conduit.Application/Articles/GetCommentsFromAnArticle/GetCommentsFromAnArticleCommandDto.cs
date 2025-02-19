using Conduit.Application.Articles.Shared;

namespace Conduit.Application.Articles.GetCommentsFromAnArticle;

public sealed record GetCommentsFromAnArticleCommandDto
(
    List<GetCommentsFromAnArticleCommandDtoProps> Comments
);

public sealed record GetCommentsFromAnArticleCommandDtoProps
(
    Guid        Id,
    DateTime    CreatedAt,
    DateTime    UpdatedAt,
    string      Body,
    AuthorModel Author
);
