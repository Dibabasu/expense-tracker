using expensetracker.api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace expensetracker.api.Application.Common.Interfaces;

public interface IExpenseDbContext
{
    DbSet<Expense> Expenses { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChanges();
    DbSet<T> Set<T>() where T : class;
    void Dispose();
}