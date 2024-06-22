using expensetracker.api.Domain.Common;
using expensetracker.api.Domain.Entities;

namespace expensetracker.api.Persistence.Repositories;

public interface IExpenseRepository : IRepository<Expense>
{
    Task<decimal> CalculateTotalExpense(DateTime startDate, DateTime endDate, Category? category);
}