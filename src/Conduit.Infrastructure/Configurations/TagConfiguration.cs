// using Conduit.Domain.Articles;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;

// namespace Conduit.Infrastructure.Configurations;

// internal sealed class ArticleConfiguration : IEntityTypeConfiguration<Article>
// {
//     public void Configure(EntityTypeBuilder<Article> builder)
//     {
//         builder.ToTable("articles");

//         builder.HasKey(article => article.Id);

//         // builder.OwnsOne(apartment => apartment.Address);

//         // builder.Property(apartment => apartment.Name)
//             // .HasMaxLength(200)
//             // .HasConversion(name => name.Value, value => new Name(value));

//         // builder.Property(apartment => apartment.Description)
//             // .HasMaxLength(2000)
//             // .HasConversion(description => description.Value, value => new Description(value));

//         // builder.OwnsOne(apartment => apartment.Price, priceBuilder =>
//         // {
//             // priceBuilder.Property(money => money.Currency)
//                 // .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
//         // });

//         // builder.OwnsOne(apartment => apartment.CleaningFee, priceBuilder =>
//         // {
//             // priceBuilder.Property(money => money.Currency)
//                 // .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
//         // });

//         // builder.Property<uint>("Version").IsRowVersion();

//                 modelBuilder
//             .Entity<Article>()
//             .HasKey(article => article.Id);

//         modelBuilder
//             .Entity<Article>()
//             .Property(article => article.Id)
//             .ValueGeneratedNever();

//         modelBuilder
//             .Entity<Article>()
//             .Property(article => article.Id)
//             .HasConversion
//             (
//                 ulid => ulid.ToString(),
//                 ulidAsString => Ulid.Parse(ulidAsString)
//             );

//                     modelBuilder
//             .Entity<Article>()
//             .HasKey(article => article.Id);

//         modelBuilder
//             .Entity<Article>()
//             .Property(article => article.Id)
//             .ValueGeneratedNever();

//         modelBuilder
//             .Entity<Article>()
//             .Property(article => article.Id)
//             .HasConversion
//             (
//                 ulid => ulid.ToString(),
//                 ulidAsString => Ulid.Parse(ulidAsString)
//             );

            

//         // modelBuilder
//         //     .Entity<Tag>()
//         //     .HasKey(t => t.Id);

//         // modelBuilder
//         //     .Entity<Tag>()
//         //     .HasIndex(t => t.Name)
//         //     .IsUnique();

//         // modelBuilder
//         //     .Entity<Tag>()
//         //     .Property(e => e.Id)
//         //     .ValueGeneratedNever()
//         //     .HasConversion(
//         //         tagEntityid => tagEntityid.Value,
//         //         valueFromDb => new TagId(valueFromDb)
//         //     );

//         // modelBuilder
//         //     .Entity<Tag>()
//         //     .Property(t => t.Name)
//         //     .HasConversion(
//         //         tagName => tagName.Value,
//         //         nameFromDb => new TagName(nameFromDb)
//         //     );






                



//         // modelBuilder
//         //     .Entity<Tag>()
//         //     .HasKey(t => t.Id);

//         // modelBuilder
//         //     .Entity<Tag>()
//         //     .HasIndex(t => t.Name)
//         //     .IsUnique();

//         // modelBuilder
//         //     .Entity<Tag>()
//         //     .Property(e => e.Id)
//         //     .ValueGeneratedNever()
//         //     .HasConversion(
//         //         tagEntityid => tagEntityid.Value,
//         //         valueFromDb => new TagId(valueFromDb)
//         //     );

//         // modelBuilder
//         //     .Entity<Tag>()
//         //     .Property(t => t.Name)
//         //     .HasConversion(
//         //         tagName => tagName.Value,
//         //         nameFromDb => new TagName(nameFromDb)
//         //     );





//     }
// }
