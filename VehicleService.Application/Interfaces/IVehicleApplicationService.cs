using VehicleService.Application.Vehicles.Dtos;

namespace VehicleService.Application.Interfaces
{
    public interface IVehicleApplicationService
    {
        Task<VehicleDto?> GetVehicleByIdAsync(Guid id);
        Task<VehicleDto?> GetVehicleByVinAsync(string vin);
        Task<IEnumerable<VehicleDto>> GetAllVehiclesAsync();
        Task<VehicleDto> CreateVehicleAsync(CreateVehicleRequest createRequest);
        Task SetVehicleStatusAsync(Guid id, Domain.Enums.VehicleStatus newStatus);
        Task DeleteVehicleAsync(Guid id);
        Task PatchVehicleAsync(Guid id, PatchVehicleRequest request);
    }
}