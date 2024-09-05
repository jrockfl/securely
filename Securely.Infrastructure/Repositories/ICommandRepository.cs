using Securely.Domain.Enities;

namespace Securely.Infrastructure.Repositories;

public interface ICommandRepository : IQueryRepository
{
    void Add<T>(T entity)
where T : Entity;

    void Delete<T>(T entity)
        where T : Entity;

    Task BeginTransactionAsync();

    Task CommitTransactionAsync();

    Task RollbackTransactionAsync();

    Task<int> SaveChangesAsync();
}
