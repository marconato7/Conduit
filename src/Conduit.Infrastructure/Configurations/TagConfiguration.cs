using Conduit.Domain.Tags;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Conduit.Infrastructure.Configurations;

internal sealed class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("tags");
        builder.HasKey(tag => tag.Id);
        builder.HasIndex(tag => tag.Name).IsUnique();
        builder.Property(tag => tag.Id).ValueGeneratedNever();
        // builder.Property<uint>("Version").IsRowVersion();
    }
}
