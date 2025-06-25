using VehicleService.Application.Interfaces;
using VehicleService.Application.Vehicles.Dtos;
using VehicleService.Domain.Entities;
using VehicleService.Domain.ValueObjects;
using VehicleService.Domain.Enums;
using AutoMapper;

namespace VehicleService.Application.Services
{
    public class VehicleApplicationService(IUnitOfWork unitOfWork, IMapper mapper) : IVehicleApplicationService
    {
        public async Task<VehicleDto?> GetVehicleByIdAsync(Guid id)
        {
            var vehicle = await unitOfWork.Vehicles.GetByIdAsync(id);
            return mapper.Map<VehicleDto?>(vehicle);
        }

        public async Task<VehicleDto?> GetVehicleByVinAsync(string vin)
        {
            var vehicle = await unitOfWork.Vehicles.GetByVinAsync(vin);
            return mapper.Map<VehicleDto?>(vehicle);
        }

        public async Task<IEnumerable<VehicleDto>> GetAllVehiclesAsync()
        {
            var vehicles = await unitOfWork.Vehicles.ListAllAsync();
            return mapper.Map<IEnumerable<VehicleDto>>(vehicles);
        }

        public async Task<VehicleDto> CreateVehicleAsync(CreateVehicleRequest request)
        {
            var basePrice = new Money(request.BasePriceAmount, request.BasePriceCurrency);

            var vehicle = Vehicle.RegisterNew(
                request.Vin,
                request.Manufacturer,
                request.Model,
                request.Package,
                request.BodyType,
                request.Year,
                request.Color,
                request.EngineType,
                request.TransmissionType,
                request.InitialMileage,
                basePriceAmount: basePrice.Amount,
                basePriceCurrency: basePrice.Currency
            );

            await unitOfWork.Vehicles.AddAsync(vehicle);
            await unitOfWork.SaveChangesAsync();

            return mapper.Map<VehicleDto>(vehicle);
        }


        public async Task SetVehicleStatusAsync(Guid id, VehicleStatus newStatus)
        {
            var vehicle = await unitOfWork.Vehicles.GetByIdAsync(id);
            if (vehicle == null)
            {
                throw new KeyNotFoundException($"Vehicle with id {id} not found.");
            }

            switch (newStatus)
            {
                case VehicleStatus.Reserved:
                    vehicle.SetReservedStatus();
                    break;
                case VehicleStatus.Sold:
                    vehicle.SetSoldStatus();
                    break;
                case VehicleStatus.Available:
                    vehicle.SetAvailableStatus();
                    break;
                default:
                    vehicle.UpdateStatus(newStatus);
                    break;
            }
            
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteVehicleAsync(Guid id)
        {
            var vehicle = await unitOfWork.Vehicles.GetByIdAsync(id);
            if (vehicle != null)
            {
                unitOfWork.Vehicles.Delete(vehicle);
                await unitOfWork.SaveChangesAsync();
            }
        }
        
    public async Task PatchVehicleAsync(Guid id, PatchVehicleRequest request)
    {
        var vehicle = await unitOfWork.Vehicles.GetByIdAsync(id);
        if (vehicle == null)
        {
            throw new KeyNotFoundException($"Vehicle with id {id} not found.");
        }
 
        if (request.Color != null) vehicle.UpdateColor(request.Color);
        if (request.Year.HasValue) vehicle.UpdateYear(request.Year.Value);
        if (request.Mileage.HasValue) vehicle.UpdateMileage(request.Mileage.Value);
        
        if (request.BasePriceAmount.HasValue && request.BasePriceCurrency != null)
        {
            var newPrice = new Money(request.BasePriceAmount.Value, request.BasePriceCurrency);
            vehicle.UpdateBasePrice(newPrice);
        }
        
        await unitOfWork.SaveChangesAsync();
    }
    
    }
}