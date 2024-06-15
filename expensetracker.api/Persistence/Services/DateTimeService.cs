using expensetracker.api.Application.Common.Interfaces;

namespace expensetracker.api.Persistence.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}