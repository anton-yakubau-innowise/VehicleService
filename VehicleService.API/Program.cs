using Microsoft.EntityFrameworkCore;
using VehicleService.API.Middleware;
using VehicleService.Application;
using VehicleService.Infrastructure;
using VehicleService.Infrastructure.Persistence;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();   

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

app.UseCustomExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<VehicleDbContext>();
        dbContext.Database.Migrate();
    }
}

app.UseAuthorization();
app.MapControllers();
app.Run();