using expensetracker.api.Application.Interfaces;
using expensetracker.api.Domain.Entities;
using expensetracker.api.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace expensetracker.api.Persistence;

public class ExpenseSqlLiteDbContext : DbContext, IExpenseDbContext
{
    private readonly IDateTime _dateTime;
    public ExpenseSqlLiteDbContext(DbContextOptions<ExpenseSqlLiteDbContext> options, IDateTime dateTime)
       : base(options)
    {
        _dateTime = dateTime;
    }
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
         modelBuilder.ApplyConfiguration(new ExpenseConfiguration());
    }
    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries<Expense>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || HasChangedOwnedEntities(e));

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = _dateTime.Now;
            }

            entry.Entity.UpdatedAt = _dateTime.Now;
        }
    }

    private bool HasChangedOwnedEntities(EntityEntry entry)
    {
        return entry.References.Any(r => r.TargetEntry != null &&
                                         r.TargetEntry.Metadata.IsOwned() &&
                                         (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
    }
}