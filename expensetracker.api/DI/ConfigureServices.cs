using expensetracker.api.Application.Common.Interfaces;
using expensetracker.api.Application.Services;
using expensetracker.api.Application.Services.Interfaces;
using expensetracker.api.Persistence;
using expensetracker.api.Persistence.Repositories;
using expensetracker.api.Persistence.Repositories.Interfaces;
using expensetracker.api.Persistence.Services;
using expensetracker.api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace expensetracker.api.DI
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IExpenseDbContext, ExpenseSqlLiteDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            // Register repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            // Register services
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<ILinkService, LinkService>();
            services.AddTransient<IDateTime, DateTimeService>();
            return services;
        }
    }
}