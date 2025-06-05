using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VehicleService.Application.Interfaces;
using VehicleService.Application.Vehicles.Dtos;
using VehicleService.Domain.Entities;
using VehicleService.Domain.Repositories;
using VehicleService.Domain.ValueObjects;
using VehicleService.Domain.Enums;

namespace VehicleService.Application.Services
{
    public class VehicleApplicationService : IVehicleApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VehicleApplicationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<VehicleDto?> GetVehicleByIdAsync(Guid id)
        {
            var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(id);
            return vehicle == null ? null : MapToVehicleDto(vehicle);
        }

        public async Task<VehicleDto?> GetVehicleByVinAsync(string vin)
        {
            var vehicle = await _unitOfWork.Vehicles.GetByVinAsync(vin);
            return vehicle == null ? null : MapToVehicleDto(vehicle);
        }

        public async Task<IEnumerable<VehicleDto>> GetAllVehiclesAsync()
        {
            var vehicles = await _unitOfWork.Vehicles.ListAllAsync();
            return vehicles.Select(MapToVehicleDto).ToList();
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

            await _unitOfWork.Vehicles.AddAsync(vehicle);
            await _unitOfWork.SaveChangesAsync();

            return MapToVehicleDto(vehicle);
        }

        
        public async Task SetVehicleStatusAsync(Guid id, VehicleStatus newStatus)
        {
            var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(id);
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
            
            _unitOfWork.Vehicles.Update(vehicle);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteVehicleAsync(Guid id)
        {
            var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(id);
            if (vehicle != null)
            {
                _unitOfWork.Vehicles.Delete(vehicle);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        private VehicleDto MapToVehicleDto(Vehicle vehicle)
        {
            return new VehicleDto(
                vehicle.Id,
                vehicle.Vin,
                vehicle.Manufacturer,
                vehicle.Model,
                vehicle.Package,
                vehicle.BodyType,
                vehicle.Year,
                vehicle.Color,
                vehicle.EngineType.ToString(),
                vehicle.TransmissionType.ToString(),
                vehicle.Mileage,
                vehicle.BasePrice.Amount,
                vehicle.BasePrice.Currency,
                vehicle.Status.ToString(),
                vehicle.CreatedAt,
                vehicle.UpdatedAt
            );
        }
    }
}