using Asp.Versioning;
using expensetracker.api.Application.Common;
using expensetracker.api.Application.DTO;
using expensetracker.api.Application.Services.Expenses;
using expensetracker.api.Application.Services.Links;
using expensetracker.api.DTO.Create;
using expensetracker.api.DTO.Get;
using expensetracker.api.DTO.Update;
using Microsoft.AspNetCore.Mvc;

namespace expensetracker.api.Controllers.v1;

[ApiVersion("1.0")]

public class ExpensesController : BaseController<ExpenseDTO>
{
    private readonly IExpenseService _expenseService;
    private readonly ILogger<ExpensesController> _logger;

    public ExpensesController(ILogger<ExpensesController> logger, IExpenseService expenseService, ILinkService linkService)
        : base(logger, linkService)
    {
        _expenseService = expenseService;
        _logger = logger;
    }

    [HttpGet()]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult<PagedResult<ExpenseDTO>>> GetExpenses(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _expenseService.GetExpenses(pageNumber, pageSize, cancellationToken);
        if (result == null) return NotFound();

        var etag = ETagHelper.GenerateETag(result);

        if (Request.Headers.TryGetValue("If-None-Match", out var requestEtag) && requestEtag == etag)
        {
            return StatusCode(304); // Not Modified
        }

        HttpContext.Items["ETag"] = etag;
        AddPaginationLinks(result, pageNumber, pageSize);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ExpenseDTO>> GetExpenses(Guid id, CancellationToken cancellationToken)
    {
        var expense = await _expenseService.GetExpenseById(id, cancellationToken);
        if (expense == null) return NotFound();

        var etag = ETagHelper.GenerateETag(expense);

        if (Request.Headers.TryGetValue("If-None-Match", out var requestEtag) && requestEtag == etag)
        {
            return StatusCode(304); // Not Modified
        }

        HttpContext.Items["ETag"] = etag;
        AddLinks(expense);
        return Ok(expense);
    }

    [HttpPost()]
    public async Task<ActionResult<ExpenseDTO>> CreateExpense([FromBody] CreateExpenseDTO expense, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var createdExpense = await _expenseService.AddExpense(expense, cancellationToken);
        var etag = ETagHelper.GenerateETag(createdExpense);

        HttpContext.Items["ETag"] = etag;
        AddLinks(createdExpense);
        return CreatedAtAction(nameof(GetExpenses), new { id = createdExpense.Id }, createdExpense);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ExpenseDTO>> UpdateExpense(Guid id, [FromBody] UpdateExpenseDTO expense, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var success = await _expenseService.UpdateExpense(id, expense, cancellationToken);
        if (!success) return NotFound();

        var updatedExpense = await _expenseService.GetExpenseById(id, cancellationToken);
        var etag = ETagHelper.GenerateETag(updatedExpense);

        HttpContext.Items["ETag"] = etag;
        AddLinks(updatedExpense);
        return Ok(updatedExpense);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteExpenses(Guid id, CancellationToken cancellationToken)
    {
        var success = await _expenseService.DeleteExpense(id, cancellationToken);
        if (!success) return NotFound();

        return NoContent();
    }
}
