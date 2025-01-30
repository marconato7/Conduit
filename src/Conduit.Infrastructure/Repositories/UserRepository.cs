using Conduit.Domain.Users;
using Conduit.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Infrastructure.Repositories;

public sealed class UserRepository(ApplicationDbContext applicationDbContext) : IUserRepository
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public void Add(User user)
    {
        _applicationDbContext
            .Set<User>()
            .Add(user);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _applicationDbContext
            .Set<User>()
            .Include(user => user.Following)
            .SingleOrDefaultAsync(user => user.Email == email, cancellationToken: cancellationToken);

        return user;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _applicationDbContext
            .Set<User>()
            .SingleOrDefaultAsync(user => user.Id == id, cancellationToken: cancellationToken);

        return user;
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var user = await _applicationDbContext
            .Set<User>()
            .SingleOrDefaultAsync(user => user.Username == username, cancellationToken: cancellationToken);

        return user;
    }
}
