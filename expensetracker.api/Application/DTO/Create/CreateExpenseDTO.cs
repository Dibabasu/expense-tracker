using expensetracker.api.Domain.Common;

namespace expensetracker.api.DTO.Create;
public class CreateExpenseDTO
{
     public string Description { get; set; } = "";
        public Category Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
}