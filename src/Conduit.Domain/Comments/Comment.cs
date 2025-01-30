// using Conduit.Domain.Abstractions;
// using Conduit.Domain.Articles;
// using Conduit.Domain.Articles.Events;

// namespace Conduit.Domain.Comments;

// public sealed class Comment
// {
//     public Guid      Id           { get; private set; } // refactor: solve primitive obsession
//     public DateTime  CreatedAtUtc { get; private set; } // refactor: solve primitive obsession
//     public DateTime? UpdatedAtUtc { get; private set; } // refactor: solve primitive obsession
//     public string    Body         { get; private set; } // refactor: solve primitive obsession
//     public string    Author       { get; private set; } // refactor: solve primitive obsession

//     private Comment() {} // for EF Core

//     public Comment(string title, string description, string body)
//     {
//         // refactor: add validation
//         Title       = title;
//         Description = description;
//         Body        = body;
//         Slug        = title;
//         Id          = Guid.CreateVersion7();
//     }

//     public static Article Create(string title, string description, string body)
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
// // namespace aspnet.webapi.Entities;

// // public class Comment
// // {
// //     // "createdAt": "2016-02-18T03:22:56.637Z",
// //     // "updatedAt": "2016-02-18T03:22:56.637Z",
// //     // "author": {
// //     //   "username": "jake",
// //     //   "bio": "I work at statefarm",
// //     //   "image": "https://i.stack.imgur.com/xHWG8.jpg",
// //     //   "following": false
// //     // }
// // //   }

// //     public Guid Id { get; private set; }
// //     public string Body { get; private set; } = null!;
// //     public User Author { get; set; } = null!;

// //     public static Comment Create(string body, string username, string password, string? bio, string? image)
// //     {
// //         return new(body);
// //     }

// //     private Comment(string body)
// //     {
// //         Id = Guid.NewGuid();
// //         Body = body;
// //     }
// // }
