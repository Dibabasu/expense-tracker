using expensetracker.api.Application.DTO;
using expensetracker.api.Application.Services.Interfaces;
using expensetracker.api.Domain.Common;
using expensetracker.api.Domain.Entities;
using expensetracker.api.Domain.ValueObjects;
using expensetracker.api.DTO.Create;
using expensetracker.api.DTO.Get;
using expensetracker.api.DTO.Update;
using expensetracker.api.Persistence.Repositories.Interfaces;
using expensetracker.api.Services.Interfaces;

namespace expensetracker.api.Application.Services;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly ILinkService _linkService;
    private readonly ILogger<ExpenseService> _logger;

    public ExpenseService(ILogger<ExpenseService> logger, IExpenseRepository expenseRepository, ILinkService linkService)
    {
        _logger = logger;
        _expenseRepository = expenseRepository;
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

            await _expenseRepository.AddAsync(newExpense);
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
            return await _expenseRepository.CalculateTotalExpense(startDate, endDate, category);
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
            var expense = await _expenseRepository.GetByIdAsync(id);
            if (expense == null) return null;

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
            var pagedExpenses = await _expenseRepository.GetPagedAsync(pageNumber, pageSize);
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
            var existingExpense = await _expenseRepository.GetByIdAsync(id);
            if (existingExpense == null) return false;

            existingExpense.Update(expense.Category, new Money(expense.Amount, "USD"), expense.Date, expense.Description);
            await _expenseRepository.UpdateAsync(existingExpense);
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
            var expense = await _expenseRepository.GetByIdAsync(id);
            if (expense == null) return false;

            await _expenseRepository.DeleteAsync(expense);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting expense");
            throw;
        }
    }


}
