using expensetracker.api.Application.Common.Interfaces;
using expensetracker.api.Persistence.Repositories;
using expensetracker.api.Persistence.Repositories.Interfaces;

namespace expensetracker.api.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IExpenseDbContext _context;
        private IExpenseRepository _expenseRepository;

        public UnitOfWork(IExpenseDbContext context)
        {
            _context = context;
        }

        public IExpenseRepository Expenses => _expenseRepository ??= new ExpenseRepository(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}