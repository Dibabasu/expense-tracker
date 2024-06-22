using System.Linq.Expressions;
using expensetracker.api.Application.Common.Interfaces;
using expensetracker.api.Application.DTO;
using Microsoft.EntityFrameworkCore;

namespace expensetracker.api.Persistence.Repositories;
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly IExpenseDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(IExpenseDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<PagedResult<T>> GetPagedAsync(int pageNumber, int pageSize)
    {
        return await PagedResult<T>.GetPagedResultAsync(_dbSet, pageNumber, pageSize);
    }

    public async Task<PagedResult<T>> FindAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize)
    {
        var query = _dbSet.Where(predicate);
        return await PagedResult<T>.GetPagedResultAsync(query, pageNumber, pageSize);
    }
}