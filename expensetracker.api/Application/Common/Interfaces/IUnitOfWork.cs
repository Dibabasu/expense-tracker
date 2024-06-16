using expensetracker.api.Persistence.Repositories.Interfaces;

namespace expensetracker.api.Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IExpenseRepository Expenses { get; }
    Task<int> CompleteAsync();
}