using Conduit.Domain.Articles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Conduit.Infrastructure.Configurations;

internal sealed class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("articles");
        builder.HasKey(article => article.Id);
        builder.Property(article => article.Id).ValueGeneratedNever();
        builder.HasIndex(article => article.Slug).IsUnique();
        // builder.Property<uint>("Version").IsRowVersion();
        // builder.HasMany(article => article.TagList).WithMany(tags => tags.Articles);
    }
}
