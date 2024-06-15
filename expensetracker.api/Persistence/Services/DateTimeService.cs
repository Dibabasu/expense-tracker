using expensetracker.api.Application.Interfaces;

namespace expensetracker.api.Persistence.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}