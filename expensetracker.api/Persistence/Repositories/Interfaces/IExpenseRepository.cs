using expensetracker.api.Domain.Common;
using expensetracker.api.Domain.Entities;

namespace expensetracker.api.Persistence.Repositories.Interfaces;

public interface IExpenseRepository : IRepository<Expense>
{
    Task<decimal> CalculateTotalExpense(DateTime startDate, DateTime endDate, Category? category);
}