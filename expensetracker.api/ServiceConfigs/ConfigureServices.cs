

using Asp.Versioning;
using expensetracker.api.Application.Common.Interfaces;
using expensetracker.api.Application.Services;
using expensetracker.api.Application.Services.Interfaces;
using expensetracker.api.Persistence;
using expensetracker.api.Persistence.Repositories;
using expensetracker.api.Persistence.Repositories.Interfaces;
using expensetracker.api.Persistence.Services;
using expensetracker.api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace expensetracker.api.ServiceConfigs;

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
        services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();
        // Register services
        services.AddScoped<IExpenseService, ExpenseService>();
        services.AddScoped<ILinkService, LinkService>();
        services.AddTransient<IDateTime, DateTimeService>();


        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new HeaderApiVersionReader("x-api-version"),
                new QueryStringApiVersionReader("api-version"),
                new UrlSegmentApiVersionReader()
            );
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });



        return services;
    }
}