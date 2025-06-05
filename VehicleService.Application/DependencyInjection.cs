using Microsoft.Extensions.DependencyInjection;
using VehicleService.Application.Interfaces;
using VehicleService.Application.Services;

namespace VehicleService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IVehicleApplicationService, VehicleApplicationService>();
            
            return services;
        }
    }
}