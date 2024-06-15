using expensetracker.api.Application.DTO;
using expensetracker.api.Domain.Common;
using expensetracker.api.DTO.Create;
using expensetracker.api.DTO.Get;
using expensetracker.api.DTO.Update;

namespace expensetracker.api.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<ExpenseDTO> AddExpense(CreateExpenseDTO expense, CancellationToken cancellationToken);
        Task<PagedResult<ExpenseDTO>> GetExpenses(int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<ExpenseDTO> GetExpenseById(Guid id, CancellationToken cancellationToken = default);

        Task<bool> DeleteExpense(Guid id, CancellationToken cancellationToken = default);
        Task<bool> UpdateExpense(Guid id, UpdateExpenseDTO expense, CancellationToken cancellationToken = default);

        Task<decimal> CalculateTotalExpense(DateTime startDate, DateTime endDate, Category? category, CancellationToken cancellationToken);
    }


}