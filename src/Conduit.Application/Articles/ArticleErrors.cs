using Conduit.Application.Articles.Shared;

namespace Conduit.Application.Articles;

public static class ArticleErrors
{
    public static ConduitError NotFound
        => new("Article not found");
}
