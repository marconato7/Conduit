using System.Data;
using Bogus;
using Conduit.Application.Abstractions.Data;
using Dapper;

namespace Conduit.Api.Extensions;

internal static class SeedDataExtensions
{
    public static void SeedData(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        ISqlConnectionFactory sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();

        using IDbConnection connection = sqlConnectionFactory.CreateConnection();

        var faker = new Faker();

        List<Guid>   ids   = [];
        List<object> users = [];

        ids.Add(Guid.CreateVersion7());
        ids.Add(Guid.CreateVersion7());
        ids.Add(Guid.CreateVersion7());
        ids.Add(Guid.CreateVersion7());
        ids.Add(Guid.CreateVersion7());
        ids.Add(Guid.CreateVersion7());
        ids.Add(Guid.CreateVersion7());
        ids.Add(Guid.CreateVersion7());
        ids.Add(Guid.CreateVersion7());
        ids.Add(Guid.CreateVersion7());

        foreach (var id in ids)
        {
            users.Add
            (
                new
                {
                    Id           = id,
                    Username     = faker.Internet.UserName(),
                    Email        = faker.Internet.Email(),
                    PasswordHash = faker.Internet.Password(),
                }
            );
        }

        const string sqlUsers =
        """
            INSERT INTO users (id, username, email, password_hash)
            VALUES (@Id, @Username, @Email, @PasswordHash);
        """;

        connection.Execute(sqlUsers, users);

        List<object> articles = [];

        var random = new Random((int) DateTime.UtcNow.Ticks);

        for (int i = 0; i < 100; i++)
        {
            articles.Add
            (
                new
                {
                    Id           = Guid.CreateVersion7(),
                    Title        = faker.Lorem.Sentence(),
                    Description  = faker.Lorem.Paragraph(),
                    Body         = faker.Lorem.Text(),
                    Slug         = faker.Lorem.Slug(),
                    AuthorId     = ids.OrderBy(x => random.Next()).Take(1).First(),
                    CreatedAtUtc = DateTime.UtcNow
                }
            );
        }

        const string sqlArticles =
        """
            INSERT INTO articles (id, title, description, body, slug, author_id, created_at_utc)
            VALUES (@Id, @Title, @Description, @Body, @Slug, @AuthorId, @CreatedAtUtc);
        """;

        connection.Execute(sqlArticles, articles);
    }
}
