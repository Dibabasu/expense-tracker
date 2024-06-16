using expensetracker.api.Domain.Common;
using expensetracker.api.Domain.ValueObjects;

namespace expensetracker.api.Domain.Entities
{
    public class Expense : Entity
    {
        public string Description { get; private set; } = string.Empty;
        public Category Category { get; private set; }
        public Money Amount { get; private set; }
        public DateTime Date { get; private set; }

        protected Expense() { }

        public Expense(Category category, Money amount, DateTime date, string description)
        {
            Id = Guid.NewGuid();
            Category = category;
            Amount = amount;
            Date = date;
            Description = description;
        }

        public void Update(Category category, Money amount, DateTime date, string description)
        {
            Category = category;
            Amount = amount;
            Date = date;
            Description = description;
        }
    }
}