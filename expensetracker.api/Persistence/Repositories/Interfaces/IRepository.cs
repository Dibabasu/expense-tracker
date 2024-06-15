using System.Linq.Expressions;
using expensetracker.api.Domain.Common;

namespace expensetracker.api.Persistence.Repositories.Interfaces;


public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<PagedResult<T>> GetAllAsync(int pageNumber, int pageSize);
    Task<PagedResult<T>> FindAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize);
}