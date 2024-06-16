using expensetracker.api.Application.Common.Interfaces;
using expensetracker.api.Application.DTO;
using expensetracker.api.Domain.Common;

namespace expensetracker.api.DTO.Get;

public class ExpenseDTO : ILinkResource
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public Category Category { get; set; }
    public string Description { get; set; }
    public List<LinkDto> Links { get; set; }
}