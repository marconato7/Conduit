namespace Conduit.Domain.Abstractions;

public interface IUnitOfWork
{
    Task<int>           SaveChangesAsync      (CancellationToken cancellationToken = default);
    // Task<DbTransaction> BeginTransactionAsync (CancellationToken cancellationToken = default);
}
