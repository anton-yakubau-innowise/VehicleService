using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using VehicleService.Application.Interfaces;
using VehicleService.Application.Services;

namespace VehicleService.Application
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            
            services.AddScoped<IVehicleApplicationService, VehicleApplicationService>();

            return services;
        }
    }
}