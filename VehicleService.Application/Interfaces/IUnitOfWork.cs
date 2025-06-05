using System.Threading;
using System.Threading.Tasks;
using VehicleService.Domain.Entities;
using VehicleService.Domain.Repositories;

namespace VehicleService.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IVehicleRepository Vehicles { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}