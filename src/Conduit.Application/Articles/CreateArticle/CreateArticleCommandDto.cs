namespace Conduit.Application.Articles.CreateArticle;

public sealed record CreateArticleCommandDto
(
    string    Title,
    string    Description,
    string    Body,
    string[]? TagList
);
