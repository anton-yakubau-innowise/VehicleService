using VehicleService.Application.Vehicles.Dtos;

namespace VehicleService.Application.Interfaces
{
    public interface IVehicleApplicationService
    {
        Task<VehicleDto?> GetVehicleByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<VehicleDto?> GetVehicleByVinAsync(string vin, CancellationToken cancellationToken = default);
        Task<IEnumerable<VehicleDto>> GetAllVehiclesAsync(CancellationToken cancellationToken = default);
        Task<VehicleDto> CreateVehicleAsync(CreateVehicleRequest createRequest, CancellationToken cancellationToken = default);
        Task ReserveVehicleAsync(Guid id, CancellationToken cancellationToken = default);
        Task MakeVehicleAvailableAsync(Guid id, CancellationToken cancellationToken = default);
        Task SellVehicleAsync(Guid id, CancellationToken cancellationToken = default);
        Task DeleteVehicleAsync(Guid id, CancellationToken cancellationToken = default);
        Task PatchVehicleAsync(Guid id, PatchVehicleRequest request, CancellationToken cancellationToken = default);
    }
}