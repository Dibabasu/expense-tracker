using Asp.Versioning;
using expensetracker.api.Application.Common;
using expensetracker.api.Application.Services.Interfaces;
using expensetracker.api.DTO.Create;
using expensetracker.api.DTO.Get;
using expensetracker.api.DTO.Update;
using expensetracker.api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace expensetracker.api.Controllers.v2;


[ApiVersion("2.0")]
[ApiController]
[Route("api/[controller]")]
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

        // Generate a combined ETag for the result
        var etag = ETagHelper.GenerateETag(result);

        // Check If-None-Match header
        if (Request.Headers.TryGetValue("If-None-Match", out var requestEtag) && requestEtag == etag)
        {
            return StatusCode(304); // Not Modified
        }

        // Set ETag in HttpContext items
        HttpContext.Items["ETag"] = etag;

        // Add pagination links
        AddPaginationLinks(result, pageNumber, pageSize);

        return Ok(result);
    }

    [HttpGet("{id}", Name = "GetExpense")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var expense = await _expenseService.GetExpenseById(id, cancellationToken);
        if (expense == null) return NotFound();

        var etag = ETagHelper.GenerateETag(expense);

        // Check If-None-Match header
        if (Request.Headers.TryGetValue("If-None-Match", out var requestEtag) && requestEtag == etag)
        {
            return StatusCode(304); // Not Modified
        }

        // Set ETag in HttpContext items
        HttpContext.Items["ETag"] = etag;
        AddLinks(expense);
        return Ok(expense);
    }

    [HttpPost(Name = "CreateExpense")]
    public async Task<IActionResult> Post([FromBody] CreateExpenseDTO expense, CancellationToken cancellationToken)
    {
        var createdExpense = await _expenseService.AddExpense(expense, cancellationToken);
        var etag = ETagHelper.GenerateETag(createdExpense);

        // Set ETag in HttpContext items
        HttpContext.Items["ETag"] = etag;
        AddLinks(createdExpense);
        return CreatedAtAction(nameof(Get), new { id = createdExpense.Id }, createdExpense);
    }

    [HttpPut("{id}", Name = "UpdateExpense")]
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateExpenseDTO expense, CancellationToken cancellationToken)
    {
        var success = await _expenseService.UpdateExpense(id, expense, cancellationToken);
        if (!success) return NotFound();

        var updatedExpense = await _expenseService.GetExpenseById(id, cancellationToken);
        var etag = ETagHelper.GenerateETag(updatedExpense);

        // Set ETag in HttpContext items
        HttpContext.Items["ETag"] = etag;
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
