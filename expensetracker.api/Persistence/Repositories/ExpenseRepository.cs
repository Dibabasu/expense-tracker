using expensetracker.api.Application.Common.Interfaces;
using expensetracker.api.Domain.Common;
using expensetracker.api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace expensetracker.api.Persistence.Repositories;

public class ExpenseRepository : Repository<Expense>, IExpenseRepository
{
    public ExpenseRepository(IExpenseDbContext context) : base(context) { }

    public async Task<decimal> CalculateTotalExpense(DateTime startDate, DateTime endDate, Category? category)
    {
        var query = _dbSet
        .Where(e => (startDate == default || e.Date >= startDate) &&
                    (endDate == default || e.Date <= endDate) &&
                    (category == null || e.Category == category))
        .Select(e => e.Amount.Amount);

        var expenses = await query.ToListAsync();

        // Sum amounts in memory
        var totalExpense = expenses.Sum(amount => Convert.ToDecimal(amount));

        return totalExpense;
    }
}