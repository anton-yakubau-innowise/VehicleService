using Microsoft.EntityFrameworkCore;
using VehicleService.API.Middleware;
using VehicleService.API;
using VehicleService.Application;
using VehicleService.Infrastructure;
using VehicleService.Infrastructure.Persistence;
using Serilog;
using VehicleService.API.Services;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up application");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(8080, listenOptions =>
        {
            listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
        });
    });

    builder.Services.AddApiServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddApplicationServices();


    var app = builder.Build();

    app.UseSerilogRequestLogging();
    app.UseCustomExceptionHandler();
    app.MapGrpcService<VehicleGrpcService>();

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

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

