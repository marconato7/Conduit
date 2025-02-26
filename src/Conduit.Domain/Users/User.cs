using Conduit.Domain.Abstractions;
using Conduit.Domain.Articles;
using Conduit.Domain.Users.Events;

namespace Conduit.Domain.Users;

public sealed class User : Entity
{
    public string  Username     { get; private set; }                 // refactor: solve primitive obsession
    public string  Email        { get; private set; }                 // refactor: solve primitive obsession
    public string  PasswordHash { get; private set; } = string.Empty; // refactor: solve primitive obsession
    public string? Bio          { get; private set; } = string.Empty; // refactor: solve primitive obsession
    public string? Image        { get; private set; } = string.Empty; // refactor: solve primitive obsession

    public List<Article> AuthoredArticles { get; set; } = [];
    public List<Article> FavoriteArticles { get; set; } = [];

    public List<User> Followers { get; } = [];
    public List<User> Following { get; } = [];

    public List<Comment> Comments { get; } = [];

    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private User() {} // for EF Core
    #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public User
    (
        string username,
        string email
    )
    {
        Username     = username;
        Email        = email;
        Id           = Guid.CreateVersion7();
    }

    public static User Create
    (
        string username,
        string email
    )
    {
        // refactor: add validation, guards, etc.
        var user = new User
        (
            username,
            email
        );

        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

        return user;
    }

    public void FavoriteArticle(Article favoriteArticle)
    {
        FavoriteArticles.Add(favoriteArticle);
    }

    public void FavoritizeArticles(List<Article> favoriteArticles)
    {
        FavoriteArticles.AddRange(favoriteArticles);
    }

    public void UnfavoriteArticle(Article favoriteArticle)
    {
        FavoriteArticles.Remove(favoriteArticle);
    }

    public void RemoveArticle(Article authoredArticle)
    {
        AuthoredArticles.Remove(authoredArticle);
    }

    public void SetPasswordHash(string passwordHash)
    {
        PasswordHash = passwordHash;
    }

    public void FollowUser(User userToFollow)
    {
        Following.Add(userToFollow);
    }

    public void AddFollower(User follower)
    {
        Followers.Add(follower);
    }

    public void UnfollowUser(User userToUnfollow)
    {
        Following.Remove(userToUnfollow);
    }

    public void Update
    (
        string  email,
        string  username,
        string  passwordHash,
        string? bio,
        string? image
    )
    {
        Email        = email;
        Username     = username;
        PasswordHash = passwordHash;
        Bio          = bio;
        Image        = image;
    }

    public bool IsFollowing(User user)
    {
        return Following.Any(u => u.Id == user.Id);
    }
}
