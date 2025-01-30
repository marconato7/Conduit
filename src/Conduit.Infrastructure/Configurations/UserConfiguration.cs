using Conduit.Domain.Articles;
using Conduit.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Conduit.Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(user => user.Id);

        builder.HasIndex(user => user.Email).IsUnique();
        builder.HasIndex(user => user.Username).IsUnique();
        builder.Property(user => user.Id).ValueGeneratedNever();

        builder
            .HasMany(u => u.AuthoredArticles)
            .WithOne(aa => aa.Author)
            .HasForeignKey(aa => aa.AuthorId)
            .IsRequired();
        
        builder
            .HasMany(user => user.FavoriteArticles)
            .WithMany(article => article.UsersThatFavorited)
            .UsingEntity(
                "FavoritedArticles", l =>
                    l
                        .HasOne(typeof(Article))
                        .WithMany()
                        .HasForeignKey("ArticlesId")
                        .HasPrincipalKey
                        (
                            nameof(Article.Id)),
                            r => r.HasOne(typeof(User)).WithMany().HasForeignKey("UsersId").HasPrincipalKey(nameof(User.Id)),
                            j => j.HasKey("UsersId", "ArticlesId"
                        )
            );
    }
}
