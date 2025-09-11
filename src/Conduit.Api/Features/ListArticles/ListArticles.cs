using Conduit.Api.Data;
using Conduit.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Api.Features.ListArticles;

public static partial class ListArticles
{
    public static void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder
            .MapGet
            (
                "api/articles",
                async (
                    ApplicationDbContext applicationDbContext,
                    [FromQuery] string? tag,
                    CancellationToken cancellationToken
                ) =>
                {
                    IQueryable<Article> query = applicationDbContext
                        .Set<Article>()
                        .AsNoTracking();

                    if (tag is not null)
                    {
                        query = query
                            .Include(a => a.Tags)
                            .Where(a => a.Tags.Any(t => t.Name == tag));
                    }

                    var articles = await query
                        .Select(a => new ArticleDto(a.Id))
                        .ToListAsync(cancellationToken: cancellationToken);

                    return Results.Ok(articles);
                }
            )
            .AllowAnonymous();
    }
}
