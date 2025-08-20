using System.Data;

namespace AurumPay.Domain.Interfaces;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    bool HasActiveTransaction { get; }
}
