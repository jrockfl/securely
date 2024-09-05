using Microsoft.EntityFrameworkCore;

using Securely.Domain.Enities;

namespace Securely.Infrastructure.Repositories;
public class QueryRepository : IQueryRepository
{
    private readonly SecurelyDbContext _context;

    private bool _disposed;

    public QueryRepository(SecurelyDbContext context)
    {
        this._context = context;
    }

    public async Task<T> GetAsync<T>(Guid id)
        where T : Entity
    {
        return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public IQueryable<T> Query<T>()
        where T : Entity
    {
        return _context.Set<T>().AsNoTracking();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _context?.Dispose();
        }

        _disposed = true;
    }
}
