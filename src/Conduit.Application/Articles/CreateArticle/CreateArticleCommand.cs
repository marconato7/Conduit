using Conduit.Application.Abstractions.Cqrs;
using FluentValidation;

namespace Conduit.Application.Articles.CreateArticle;

public sealed record CreateArticleCommand
(
    string        CurrentUsersEmail,
    string        Title,
    string        Description,
    string        Body,
    List<string>? TagList
) : ICommand<CreateArticleCommandDto>;


public class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
{
    public CreateArticleCommandValidator()
    {
        RuleFor(cac => cac.CurrentUsersEmail)
            .NotEmpty();

        RuleFor(cac => cac.Title)
            .NotEmpty();

        RuleFor(cac => cac.Description)
            .NotEmpty();

        RuleFor(cac => cac.Body)
            .NotEmpty();
    }
}
