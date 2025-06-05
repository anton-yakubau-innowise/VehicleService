using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
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
        void Update(Vehicle vehicle);
        void Delete(Vehicle vehicle);
    }
}