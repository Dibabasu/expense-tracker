using expensetracker.api.Middleware;
using expensetracker.api.ServiceConfigs;

var builder = WebApplication.CreateBuilder(args);
{

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
       {
           builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
       }));

    builder.Services.AddServices(builder.Configuration);
}

var app = builder.Build();
{

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    // Apply the CORS middleware
    app.UseCors("corsapp");
    app.UseMiddleware<ETagMiddleware>();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}