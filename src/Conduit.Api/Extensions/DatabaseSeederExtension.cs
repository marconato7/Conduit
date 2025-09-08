using System.Data;
using Bogus;
using Conduit.Api.Data;
using Conduit.Application.Abstractions.Authentication;
using Conduit.Application.Abstractions.Data;
using Conduit.Domain.Articles;
using Conduit.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Api.Extensions;

internal static class DatabaseSeederExtension
{
    public static async void SeedData(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        ISqlConnectionFactory sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
        ITokenService         tokenService         = scope.ServiceProvider.GetRequiredService<ITokenService>();
        ApplicationDbContext  applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        using IDbConnection   connection           = sqlConnectionFactory.CreateConnection();
        var                   passwordHasher       = new PasswordHasher<User>();
        var                   random               = new Random((int) DateTime.UtcNow.Ticks);

        // Users
        var faker = new Faker();

        List<User> users = [];

        List<UserInfo> usersInfo =
        [
            new("annie",   "annie@annie.annie",   "annie@annie.annie" ),
            new("barbara", "barbara@barbara.barbara", "barbara@barbara.barbara" ),
            new("katie",   "katie@katie.katie",   "katie@katie.katie" ),
            new("debora",  "debora@debora.debora",  "debora@debora.debora" ),
            new("emily",   "emily@emily.emily",   "emily@emily.emily" ),
            new("fanny",   "fanny@fanny.fanny",   "fanny@fanny.fanny" ),
            new("greta",   "greta@greta.greta",   "greta@greta.greta" ),
            new("hannah",  "hannah@hannah.hannah",  "hannah@hannah.hannah" ),
            new("irulian", "irulian@irulian.irulian", "irulian@irulian.irulian" ),
            new("jane",    "jane@jane.jane",    "jane@jane.jane" ),
        ];

        foreach (var userInfo in usersInfo)
        {
            var user = User.Create
            (
                userInfo.UserName,
                userInfo.Email
            );

            var passwordHash = passwordHasher.HashPassword
            (
                user,
                userInfo.Password
            );

            user.SetPasswordHash(passwordHash);

            users.Add(user);
        }

        // Have users follow each other
        foreach (var outterUser in users)
        {
            foreach (var innerUser in users)
            {
                if (outterUser.Id.Equals(innerUser.Id))
                {
                    continue;
                }

                outterUser.FollowUser(innerUser);

                applicationDbContext.Attach(outterUser);
                applicationDbContext.Attach(innerUser);

                applicationDbContext.Entry(outterUser).State = EntityState.Modified;
                applicationDbContext.Entry(innerUser).State = EntityState.Modified;
            }
        }

        // Tags
        List<string> tagsNames =
        [
            "php",
            "c#",
            "javascript",
            "java",
            "html",
            "css",
            "python",
            "sql",
            "mysql",
            "postgresql",
            "sqlite",
            "dns",
            "http",
        ];

        List<Tag> tags = [];

        foreach (var tagName in tagsNames)
        {
            tags.Add(Tag.Create(tagName));
        }

        await applicationDbContext.AddRangeAsync(tags);

        // Articles
        List<Article> articles = [];

        for (int i = 0; i < 100; i++)
        {
            articles.Add
            (
                Article.Create
                (
                    title:                   faker.Lorem.Sentence(),
                    description:             faker.Lorem.Paragraph(),
                    body:                    faker.Lorem.Text(),
                    author:                  users.OrderBy(x => random.Next()).Take(1).First(),
                    createdAtUtc:            DateTime.UtcNow,
                    tagsThatNeedToBeCreated: [],
                    existingTags:            [.. tags.OrderBy(x => random.Next()).Take(3)]
                )
            );
        }

        // Add comments to the articles
        foreach (var article in articles)
        {
            int numberOfComments = random.Next(1, 11);
            List<Comment> comments = [];

            for (int i = 0; i < numberOfComments; i++)
            {
                var comment = Comment.Create
                (
                    body:         faker.Lorem.Sentence(),
                    createdAtUtc: DateTime.UtcNow,
                    author:       users.OrderBy(x => random.Next()).Take(1).First()
                );

                comments.Add(comment);
            }

            article.AddComments(comments);
        }

        await applicationDbContext.AddRangeAsync(articles);

        // Have users favorite articles
        foreach (var user in users)
        {
            int numberOfArticlesToBeFavoritized = random.Next(1, 11);

            var listOfarticlesToBeFavoritized = articles
                .OrderBy(x => random.Next())
                .Take(numberOfArticlesToBeFavoritized)
                .ToList();

            user.FavoritizeArticles(listOfarticlesToBeFavoritized);
        }

        await applicationDbContext.AddRangeAsync(users);

        await applicationDbContext.SaveChangesAsync();
    }
}
