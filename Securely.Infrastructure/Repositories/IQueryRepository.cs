using Securely.Domain.Enities;

namespace Securely.Infrastructure.Repositories;

public interface IQueryRepository : IDisposable
{
    Task<T> GetAsync<T>(Guid id)
        where T : Entity;

    IQueryable<T> Query<T>()
        where T : Entity;
}
