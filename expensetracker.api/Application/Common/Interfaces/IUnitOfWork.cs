using expensetracker.api.Persistence.Repositories;

namespace expensetracker.api.Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IExpenseRepository Expenses { get; }
    Task<int> CompleteAsync();
}