using Asp.Versioning;
using expensetracker.api.Application.Common;
using expensetracker.api.Application.DTO;
using expensetracker.api.Application.Services.Interfaces;
using expensetracker.api.DTO.Create;
using expensetracker.api.DTO.Get;
using expensetracker.api.DTO.Update;
using expensetracker.api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace expensetracker.api.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : BaseController<ExpenseDTO>
    {
        private readonly IExpenseService _expenseService;
        private readonly ILogger<ExpenseController> _logger;

        public ExpenseController(ILogger<ExpenseController> logger, IExpenseService expenseService, ILinkService linkService)
            : base(logger, linkService)
        {
            _expenseService = expenseService;
            _logger = logger;
        }

        [HttpGet(Name = "GetExpenses")]
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

        [HttpGet("{id}", Name = "GetExpense")]
        public async Task<ActionResult<ExpenseDTO>> Get(Guid id, CancellationToken cancellationToken)
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

        [HttpPost(Name = "CreateExpense")]
        public async Task<ActionResult<ExpenseDTO>> Post([FromBody] CreateExpenseDTO expense, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdExpense = await _expenseService.AddExpense(expense, cancellationToken);
            var etag = ETagHelper.GenerateETag(createdExpense);

            HttpContext.Items["ETag"] = etag;
            AddLinks(createdExpense);
            return CreatedAtAction(nameof(Get), new { id = createdExpense.Id }, createdExpense);
        }

        [HttpPut("{id}", Name = "UpdateExpense")]
        public async Task<ActionResult<ExpenseDTO>> Put(Guid id, [FromBody] UpdateExpenseDTO expense, CancellationToken cancellationToken)
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

        [HttpDelete("{id}", Name = "DeleteExpense")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var success = await _expenseService.DeleteExpense(id, cancellationToken);
            if (!success) return NotFound();

            return NoContent();
        }
    }
}
