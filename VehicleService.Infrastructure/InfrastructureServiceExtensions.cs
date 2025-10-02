using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VehicleService.Application.Interfaces;
using VehicleService.Domain.Repositories;
using VehicleService.Infrastructure.Persistence;
using VehicleService.Infrastructure.Persistence.Repositories;
using VehicleService.Infrastructure.Storage;

namespace VehicleService.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<VehicleDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.Configure<StorageSettings>(configuration.GetSection(StorageSettings.SectionName));

        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IFileStorageService, AzureBlobStorageService>();

        return services;
    }
}