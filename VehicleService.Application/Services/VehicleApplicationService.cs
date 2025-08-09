using VehicleService.Application.Interfaces;
using VehicleService.Application.Vehicles.Dtos;
using VehicleService.Domain.Entities;
using VehicleService.Domain.ValueObjects;
using VehicleService.Domain.Enums;
using AutoMapper;
using Microsoft.Extensions.Logging;
using VehicleService.Domain.Common;

namespace VehicleService.Application.Services
{
    public class VehicleApplicationService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<VehicleApplicationService> logger) : IVehicleApplicationService
    {

        public async Task<VehicleDto?> GetVehicleByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Validating vehicle ID and retrieving vehicle with ID: {VehicleId}", id);
            Guard.AgainstEmptyGuid(id, nameof(id));
            var vehicle = await unitOfWork.Vehicles.GetByIdAsync(id, cancellationToken);

            if (vehicle is null)
            {
                logger.LogWarning("Vehicle with ID {VehicleId} was not found in the database.", id);
                return null;
            }

            logger.LogInformation("Successfully retrieved vehicle with ID {VehicleId}.", id);
            return mapper.Map<VehicleDto>(vehicle);
        }

        public async Task<VehicleDto?> GetVehicleByVinAsync(string vin, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Validating VIN and retrieving vehicle with VIN: {Vin}", vin);
            Guard.AgainstNullOrWhiteSpace(vin, nameof(vin));
            var vehicle = await unitOfWork.Vehicles.GetByVinAsync(vin, cancellationToken);

            if (vehicle is null)
            {
                logger.LogWarning("Vehicle with VIN: {Vin} not found", vin);
                return null;
            }

            logger.LogInformation("Successfully retrieved vehicle with VIN: {Vin}.", vin);
            return mapper.Map<VehicleDto?>(vehicle);
        }

        public async Task<IEnumerable<VehicleDto>> GetAllVehiclesAsync(CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Retrieving all vehicles");
            var vehicles = await unitOfWork.Vehicles.ListAllAsync(cancellationToken);
            var vehicleList = vehicles.ToList();
            
            logger.LogInformation("Successfully retrieved {VehicleCount} vehicles", vehicleList.Count);
            return mapper.Map<IEnumerable<VehicleDto>>(vehicleList);
        }

        public async Task<VehicleDto> CreateVehicleAsync(CreateVehicleRequest request, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Attempting to create a new vehicle with VIN {Vin}", request.Vin);

            var basePrice = new Money(request.BasePriceAmount, request.BasePriceCurrency);

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
            logger.LogInformation("Vehicle with ID {VehicleId} was created and added successfully.", vehicle.Id);
            return mapper.Map<VehicleDto>(vehicle);
        }

        public async Task MakeVehicleAvailableAsync(Guid id, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Attempting to make vehicle with ID {VehicleId} available", id);
            var vehicle = await GetVehicleAndEnsureExistsAsync(id, cancellationToken);

            vehicle.SetAvailableStatus();

            await unitOfWork.SaveChangesAsync();
            logger.LogInformation("Successfully made vehicle with ID {VehicleId} available", id);
        }

        public async Task ReserveVehicleAsync(Guid id, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Attempting to reserve vehicle with ID {VehicleId}", id);
            var vehicle = await GetVehicleAndEnsureExistsAsync(id, cancellationToken);

            vehicle.SetReservedStatus();

            await unitOfWork.SaveChangesAsync();
            logger.LogInformation("Successfully reserved vehicle with ID {VehicleId}", id);
        }
        
        public async Task SellVehicleAsync(Guid id, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Attempting to sell vehicle with ID {VehicleId}", id);
            var vehicle = await GetVehicleAndEnsureExistsAsync(id, cancellationToken);

            vehicle.SetSoldStatus();

            await unitOfWork.SaveChangesAsync();
            logger.LogInformation("Successfully sold vehicle with ID {VehicleId}", id);
        }

        public async Task DeleteVehicleAsync(Guid id, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Attempting to delete vehicle with ID {VehicleId}", id);
            var vehicle = await GetVehicleAndEnsureExistsAsync(id, cancellationToken);

            unitOfWork.Vehicles.Delete(vehicle);

            await unitOfWork.SaveChangesAsync();
            logger.LogInformation("Vehicle with ID: {VehicleId} deleted successfully", id);
        }

        public async Task PatchVehicleAsync(Guid id, PatchVehicleRequest request, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Attempting to patch vehicle with ID {VehicleId}", id);
            var vehicle = await GetVehicleAndEnsureExistsAsync(id, cancellationToken);

            Money? newBasePrice = (request.BasePriceAmount.HasValue && !string.IsNullOrEmpty(request.BasePriceCurrency))
                ? new Money(request.BasePriceAmount.Value, request.BasePriceCurrency)
                : null;
                
            vehicle.UpdateDetails(
                request.Color,
                request.Year,
                request.Mileage,
                newBasePrice
            );

            await unitOfWork.SaveChangesAsync();
            logger.LogInformation("Vehicle with ID: {VehicleId} patched successfully", id);
        }

        private async Task<Vehicle> GetVehicleAndEnsureExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            Guard.AgainstEmptyGuid(id, nameof(id));
            var vehicle = await unitOfWork.Vehicles.GetByIdAsync(id, cancellationToken);

            if (vehicle is null)
            {
                logger.LogWarning("Expected vehicle with ID: {VehicleId} not found", id);
                throw new KeyNotFoundException($"Vehicle with ID {id} not found.");
            }

            logger.LogDebug("Successfully retrieved vehicle with ID {VehicleId} for modification", id);
            return vehicle;
        }

    }
}
