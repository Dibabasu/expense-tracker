using expensetracker.api.Application.Common.Interfaces;
using expensetracker.api.Application.DTO;
using expensetracker.api.Application.Services.Links;
using expensetracker.api.Domain.Common;
using expensetracker.api.Domain.Entities;
using expensetracker.api.Domain.ValueObjects;
using expensetracker.api.DTO.Create;
using expensetracker.api.DTO.Get;
using expensetracker.api.DTO.Update;

namespace expensetracker.api.Application.Services.Expenses;

public class ExpenseService : IExpenseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILinkService _linkService;
    private readonly ILogger<ExpenseService> _logger;

    public ExpenseService(ILogger<ExpenseService> logger, IUnitOfWork unitOfWork, ILinkService linkService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _linkService = linkService;
    }

    public async Task<ExpenseDTO> AddExpense(CreateExpenseDTO expense, CancellationToken cancellationToken)
    {
        try
        {
            var newExpense = new Expense(
        category: expense.Category,
         amount: new Money(expense.Amount, "USD"),
                date: expense.Date,
         description: expense.Description);

            await _unitOfWork.Expenses.AddAsync(newExpense);
            await _unitOfWork.CompleteAsync();

            var expenseDto = MapToDTO(newExpense);
            expenseDto.Links = _linkService.GenerateLinks<ExpenseDTO>(newExpense.Id);
            return expenseDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding expense");
            throw;
        }
    }

    public async Task<decimal> CalculateTotalExpense(DateTime startDate, DateTime endDate, Category? category, CancellationToken cancellationToken)
    {
        try
        {
            return await _unitOfWork.Expenses.CalculateTotalExpense(startDate, endDate, category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating total expense: {message}", ex.Message);
            return 0;
        }
    }

    public async Task<ExpenseDTO> GetExpenseById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var expense = await _unitOfWork.Expenses.GetByIdAsync(id)
             ?? throw new ArgumentException($"Expense with id {id} not found");

            var expenseDto = MapToDTO(expense);
            expenseDto.Links = _linkService.GenerateLinks<ExpenseDTO>(expense.Id);
            return expenseDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving expense");
            throw;
        }
    }

    public async Task<PagedResult<ExpenseDTO>> GetExpenses(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        try
        {
            var pagedExpenses = await _unitOfWork.Expenses.GetPagedAsync(pageNumber, pageSize);
            var expenseDtos = pagedExpenses.Items.Select(MapToDTO).ToList();
            foreach (var expenseDto in expenseDtos)
            {
                expenseDto.Links = _linkService.GenerateLinks<ExpenseDTO>(expenseDto.Id);
            }

            return new PagedResult<ExpenseDTO>(expenseDtos, pagedExpenses.TotalCount, pagedExpenses.PageSize, pagedExpenses.PageNumber)
            {
                Links = expenseDtos.SelectMany(e => e.Links).ToList()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving expenses");
            throw;
        }
    }

    public async Task<bool> UpdateExpense(Guid id, UpdateExpenseDTO expense, CancellationToken cancellationToken)
    {
        try
        {
            var existingExpense = await _unitOfWork.Expenses.GetByIdAsync(id);
            if (existingExpense == null) return false;

            existingExpense.Update(expense.Category, new Money(expense.Amount, "USD"), expense.Date, expense.Description);
            await _unitOfWork.Expenses.UpdateAsync(existingExpense); await _unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating expense");
            throw;
        }
    }

    private ExpenseDTO MapToDTO(Expense expense)
    {
        return new ExpenseDTO
        {
            Id = expense.Id,

            Amount = expense.Amount.Amount,
            Date = expense.Date,
            Category = expense.Category,
            Description = expense.Description
        };
    }

    public async Task<bool> DeleteExpense(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var expense = await _unitOfWork.Expenses.GetByIdAsync(id);
            if (expense == null) return false; await _unitOfWork.Expenses.DeleteAsync(expense);
            await _unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting expense");
            throw;
        }
    }


}
