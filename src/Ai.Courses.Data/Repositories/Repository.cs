using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Ai.Courses.Data.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly UserDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(UserDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    // Async
    public async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public async Task<T?> GetByIdAsync(object id)
        => await _dbSet.FindAsync(id);

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.Where(predicate).ToListAsync();

    public async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);

    public async Task AddRangeAsync(IEnumerable<T> entities)
        => await _dbSet.AddRangeAsync(entities);

    public Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public void RemoveRange(IEnumerable<T> entities)
        => _dbSet.RemoveRange(entities);

    public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();
}
