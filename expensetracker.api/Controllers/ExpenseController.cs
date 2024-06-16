using expensetracker.api.Application.Services.Interfaces;
using expensetracker.api.DTO.Create;
using expensetracker.api.DTO.Get;
using expensetracker.api.DTO.Update;
using expensetracker.api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace expensetracker.api.Controllers;

[Route("[controller]")]
public class ExpenseController : BaseController<ExpenseDTO>
{
    private readonly IExpenseService _expenseService;

    public ExpenseController(ILogger<ExpenseController> logger, IExpenseService expenseService, ILinkService linkService)
        : base(logger, linkService)
    {
        _expenseService = expenseService;
    }

    [HttpGet(Name = "GetExpenses")]
    public async Task<IActionResult> GetExpenses(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _expenseService.GetExpenses(pageNumber, pageSize, cancellationToken);
        AddPaginationLinks(result, pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("{id}", Name = "GetExpense")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var expense = await _expenseService.GetExpenseById(id, cancellationToken);
        AddLinks(expense);
        return Ok(expense);
    }

    [HttpPost(Name = "CreateExpense")]
    public async Task<IActionResult> Post([FromBody] CreateExpenseDTO expense, CancellationToken cancellationToken)
    {
        var createdExpense = await _expenseService.AddExpense(expense, cancellationToken);
        AddLinks(createdExpense);
        return CreatedAtAction(nameof(Get), new { id = createdExpense.Id }, createdExpense);
    }

    [HttpPut("{id}", Name = "UpdateExpense")]
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateExpenseDTO expense, CancellationToken cancellationToken)
    {
        var success = await _expenseService.UpdateExpense(id, expense, cancellationToken);
        if (!success) return NotFound();
        var updatedExpense = await _expenseService.GetExpenseById(id, cancellationToken);
        AddLinks(updatedExpense);
        return Ok(updatedExpense);
    }

    [HttpDelete("{id}", Name = "DeleteExpense")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var success = await _expenseService.DeleteExpense(id, cancellationToken);
        if (!success) return NotFound();
        return NoContent();
    }
}
