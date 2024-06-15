using expensetracker.api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace expensetracker.api.Persistence.Common;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.OwnsOne(e => e.Amount, m =>
        {
            m.Property(m => m.Amount)
             .HasColumnName("Amount")
             .HasConversion<decimal>();
            m.Property(m => m.Currency)
             .HasColumnName("Currency")
             .HasDefaultValue("USD");
        });

    }
}