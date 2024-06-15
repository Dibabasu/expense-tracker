using expensetracker.api.Application.DTO;

namespace expensetracker.api.DTO.Get;

public class ExpenseDTO
{
    public Guid Id { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Category { get; set; } = string.Empty;
    public IList<LinkDto> Links { get; set; }
}