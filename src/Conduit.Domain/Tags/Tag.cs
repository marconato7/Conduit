// using Conduit.Domain.Abstractions;
// using Conduit.Domain.Articles;
// using Conduit.Domain.Articles.Events;

// namespace Conduit.Domain.Tags;

// public sealed class Tag
// {
//     public Guid      Id           { get; private set; } // refactor: solve primitive obsession
//     public DateTime  CreatedAtUtc { get; private set; } // refactor: solve primitive obsession
//     public DateTime? UpdatedAtUtc { get; private set; } // refactor: solve primitive obsession
//     public string    Body         { get; private set; } // refactor: solve primitive obsession
//     public string    Author       { get; private set; } // refactor: solve primitive obsession

//     private Tag() {} // for EF Core

//     public Tag
//     (
//         string title,
//         string description,
//         string body
//     )
//     {
//         // refactor: add validation
//         Title       = title;
//         Description = description;
//         Body        = body;
//         Slug        = title;
//         Id          = Guid.CreateVersion7();
//     }

//     public static Article Create
//     (
//         string title,
//         string description,
//         string body
//     )
//     {
//         // refactor: add validation
//         var article = new Article
//         (
//             title:       title,
//             description: description,
//             body:        body
//         );

//         article.RaiseDomainEvent(new ArticleCreatedDomainEvent(article.Id));

//         return article;
//     }
// }


// namespace aspnet.webapi.Entities;

// public class Tag
// {
//     // public TagId Id { get; private init; } = null!;
//     public int Id { get; private init; }
//     // public TagName Name { get; private init; } = null!;
//     public string Name { get; private init; } = null!;

//     private Tag() {}

//     private Tag(string name)
//     {
//         // Id = new TagId(Ulid.NewUlid());
//         // Name = new TagName(name);
//         Name = name;
//     }

//     public static Tag Create(string name) => new(name);
// }

// // public record TagId(Ulid Value);

// // public record TagName(string Value);
