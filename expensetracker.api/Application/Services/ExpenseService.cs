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
            _logger.LogError(ex, "Error adding expense: {message}", ex.Message);
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
        var expense = await _expenseRepository.GetByIdAsync(id) ??
                      throw new Exception($"Expense with ID {id} not found.");

        var expenseDto = MapToDTO(expense);
        expenseDto.Links = _linkService.GenerateLinks<ExpenseDTO>(id);

        return expenseDto;
    }
    public async Task<PagedResult<ExpenseDTO>> GetExpenses(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var expenses = await _expenseRepository.GetAllAsync(pageNumber, pageSize);
        var expenseDTOs = expenses.Items.Select(MapToDTO).ToList();

        foreach (var expense in expenseDTOs)
        {
            var links = _linkService.GenerateLinks<ExpenseDTO>(expense.Id);
            expense.Links = links;
        }

        var pagedResult = new PagedResult<ExpenseDTO>(expenseDTOs, expenses.TotalCount, expenses.PageSize, expenses.PageNumber);
        pagedResult.Links = _linkService.GeneratePaginationLinks<ExpenseDTO>(pageNumber, pageSize, expenses.TotalCount);

        return pagedResult;
    }

    public async Task<bool> UpdateExpense(Guid id, UpdateExpenseDTO expense, CancellationToken cancellationToken)
    {
        try
        {
            var existingExpense = await _expenseRepository.GetByIdAsync(id);
            if (existingExpense == null)
                return false;

            existingExpense.UpdateAmount(new Money(expense.Amount, "USD"));
            existingExpense.UpdateDescription(expense.Description);
            existingExpense.UpdateCategory(expense.Category);
            existingExpense.UpdateDate(expense.Date);

            await _expenseRepository.UpdateAsync(existingExpense);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating expense with ID: {id}", id);
            return false;
        }
    }

    public async Task<bool> DeleteExpense(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var existingExpense = await _expenseRepository.GetByIdAsync(id);
            if (existingExpense == null)
                return false;

            await _expenseRepository.DeleteAsync(existingExpense);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting expense with ID: {id}", id);
            return false;
        }
    }

    private static ExpenseDTO MapToDTO(Expense expense)
    {
        return new ExpenseDTO
        {
            Id = expense.Id,
            Amount = expense.Amount.Amount,
            Date = expense.Date,
            Category = expense.Category.ToString(),
            Description = expense.Description,
            Links = new List<LinkDto>()
        };
    }
}
