using VehicleService.Application.Interfaces;
using VehicleService.Application.Vehicles.Dtos;
using VehicleService.Domain.Entities;
using VehicleService.Domain.ValueObjects;
using VehicleService.Domain.Enums;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace VehicleService.Application.Services
{
    public class VehicleApplicationService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<VehicleApplicationService> logger) : IVehicleApplicationService
    {

        public async Task<VehicleDto?> GetVehicleByIdAsync(Guid id)
        {
            var vehicle = await GetVehicleAndEnsureExistsAsync(id, CancellationToken.None);
            logger.LogDebug("Mapping vehicle to DTO");
            return mapper.Map<VehicleDto?>(vehicle);
        }

        public async Task<VehicleDto?> GetVehicleByVinAsync(string vin)
        {
            var vehicle = await unitOfWork.Vehicles.GetByVinAsync(vin);
            logger.LogInformation("Retrieved vehicle with VIN: {vin}", vin);
            logger.LogDebug("Mapping vehicle to DTO");
            return mapper.Map<VehicleDto?>(vehicle);
        }

        public async Task<IEnumerable<VehicleDto>> GetAllVehiclesAsync()
        {
            var vehicles = await unitOfWork.Vehicles.ListAllAsync();
            logger.LogInformation("Retrieved {VehicleCount} vehicles.", vehicles.Count());
            logger.LogDebug("Mapping vehicles to DTOs");
            return mapper.Map<IEnumerable<VehicleDto>>(vehicles);
        }

        public async Task<VehicleDto> CreateVehicleAsync(CreateVehicleRequest request)
        {
            logger.LogDebug("Combining price amount and currency into Money value object");
            var basePrice = new Money(request.BasePriceAmount, request.BasePriceCurrency);

            logger.LogInformation("Creating new vehicle from request");
            var vehicle = Vehicle.RegisterNewVehicle(
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
            logger.LogInformation("Vehicle with ID {VehicleId} was created successfully.", vehicle.Id);

            logger.LogDebug("Mapping created vehicle to DTO");
            return mapper.Map<VehicleDto>(vehicle);
        }


        public async Task SetVehicleStatusAsync(Guid id, VehicleStatus newStatus)
        {
            var vehicle = await GetVehicleAndEnsureExistsAsync(id, CancellationToken.None);

            logger.LogDebug("Setting vehicle status to {NewStatus}", newStatus);
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
            logger.LogInformation("Vehicle with ID: {id} saved with status set to {NewStatus}", id, newStatus);
        }

        public async Task DeleteVehicleAsync(Guid id)
        {
            var vehicle = await GetVehicleAndEnsureExistsAsync(id, CancellationToken.None);

            unitOfWork.Vehicles.Delete(vehicle);
            await unitOfWork.SaveChangesAsync();
            logger.LogInformation("Vehicle with ID: {id} deleted successfully", id);
        }

        public async Task PatchVehicleAsync(Guid id, PatchVehicleRequest request)
        {
            var vehicle = await GetVehicleAndEnsureExistsAsync(id, CancellationToken.None);

            if (request.Color != null)
            {
                vehicle.UpdateColor(request.Color);
                logger.LogDebug("Updated vehicle color to {Color}", request.Color);
            }
            if (request.Year.HasValue)
            {
                vehicle.UpdateYear(request.Year.Value);
                logger.LogDebug("Updated vehicle year to {Year}", request.Year.Value);
            }
            if (request.Mileage.HasValue)
            {
                vehicle.UpdateMileage(request.Mileage.Value);
                logger.LogDebug("Updated vehicle mileage to {Mileage}", request.Mileage.Value);
            }

            if (request.BasePriceAmount.HasValue && request.BasePriceCurrency != null)
            {
                var newPrice = new Money(request.BasePriceAmount.Value, request.BasePriceCurrency);
                vehicle.UpdateBasePrice(newPrice);
                logger.LogDebug("Updated vehicle base price to {BasePrice}", newPrice);
            }

            await unitOfWork.SaveChangesAsync();
            logger.LogInformation("Vehicle with ID: {id} patched successfully", id);
        }

        private async Task<Vehicle> GetVehicleAndEnsureExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            var vehicle = await unitOfWork.Vehicles.GetByIdAsync(id, cancellationToken);

            if (vehicle is null)
            {
                logger.LogWarning("Vehicle with ID: {id} not found", id);
                throw new KeyNotFoundException($"Vehicle with ID {id} not found.");
            }

            logger.LogInformation("Retrieved vehicle with ID: {id}", id);
            return vehicle;
        }

    }
}
