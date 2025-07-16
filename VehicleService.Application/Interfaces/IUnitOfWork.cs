using VehicleService.Domain.Repositories;

namespace VehicleService.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IVehicleRepository Vehicles { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}