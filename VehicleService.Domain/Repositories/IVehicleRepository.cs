using System.Linq.Expressions;
using VehicleService.Domain.Entities;

namespace VehicleService.Domain.Repositories
{
    public interface IVehicleRepository
    {
        Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Vehicle?> GetByVinAsync(string vin, CancellationToken cancellationToken = default);
        Task<IEnumerable<Vehicle>> ListAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Vehicle>> ListAsync(Expression<Func<Vehicle, bool>> predicate, CancellationToken cancellationToken = default);
        
        Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
        void Delete(Vehicle vehicle);
    }
}