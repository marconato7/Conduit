namespace Conduit.Domain.Users;

public interface IUserRepository
{
    Task<bool> CheckIfExistsByEmail
    (
        string            email,
        CancellationToken cancellationToken = default
    );

    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailWithFollowingAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailWithAuthoredArticlesAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    void Add(User user);
}
