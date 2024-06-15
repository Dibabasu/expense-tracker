using expensetracker.api.Domain.Common;
using expensetracker.api.DTO.Create;
using expensetracker.api.DTO.Update;
using expensetracker.api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace expensetracker.api.Controllers
{
    [Route("[controller]")]
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _expenseService;
        private readonly ILogger<ExpenseController> _logger;

        public ExpenseController(ILogger<ExpenseController> logger, IExpenseService expenseService)
        {
            _logger = logger;
            _expenseService = expenseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetExpenses(int pageNumber = 1,
                                                     int pageSize = 10,
                                                     CancellationToken cancellationToken = default)
        {
            return Ok(await _expenseService.GetExpenses(pageNumber, pageSize, cancellationToken));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
        {
            var expense = await _expenseService.GetExpenseById(id, cancellationToken);
            return Ok(expense);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateExpenseDTO expense, CancellationToken cancellationToken)
        {
            return Ok(await _expenseService.AddExpense(expense,cancellationToken));
        }
        [HttpGet("totalExpense")]
        public async Task<IActionResult> CalculateExpense(DateTime startDate, DateTime endDate, Category category, CancellationToken cancellationToken)
        {
            return Ok(await _expenseService.CalculateTotalExpense(startDate, endDate, category, cancellationToken));
        }
        [HttpPut("{id}")]

        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateExpenseDTO expense)
        {
            return Ok(await _expenseService.UpdateExpense(id, expense));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _expenseService.DeleteExpense(id));
        }

    }
}