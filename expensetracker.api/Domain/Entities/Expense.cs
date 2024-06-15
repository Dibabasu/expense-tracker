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

        public Expense(string description, Category category, Money amount, DateTime date)
        {
            SetDescription(description);
            SetCategory(category);
            SetAmount(amount);
            SetDate(date);
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be empty.");
            Description = description;
        }

        public void SetCategory(Category category)
        {
            Category = category;
        }

        public void SetAmount(Money amount)
        {
            Amount = amount ?? throw new ArgumentNullException(nameof(amount));
        }

        public void SetDate(DateTime date)
        {
            Date = date;
        }
        public void UpdateDescription(string description)
        {
            SetDescription(description);
        }

        public void UpdateCategory(Category category)
        {
            SetCategory(category);
        }

        public void UpdateAmount(Money amount)
        {
            SetAmount(amount);
        }

        public void UpdateDate(DateTime date)
        {
            SetDate(date);
        }
    }
}