using Microsoft.EntityFrameworkCore;

using Securely.Domain.Enities;

namespace Securely.Infrastructure.Repositories;
public class CommandRepository : ICommandRepository
{
    private readonly SecurelyDbContext _context;

    private bool _disposed;

    public CommandRepository(SecurelyDbContext context)
    {
        _context = context;
    }

    public void Add<T>(T entity) where T : Entity
    {
        Set<T>().Add(entity);
    }

    public void Delete<T>(T entity) where T : Entity
    {
        Set<T>().Remove(entity);
    }

    public async Task BeginTransactionAsync()
    {
        await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
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

    public Task<T> GetAsync<T>(Guid id) where T : Entity
    {
        return Set<T>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public IQueryable<T> Query<T>() where T : Entity
    {
        return Set<T>();
    }

    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    private DbSet<T> Set<T>() where T : class
    {
        return _context.Set<T>();
    }
}
