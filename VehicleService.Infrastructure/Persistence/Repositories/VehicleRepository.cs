using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VehicleService.Domain.Entities;
using VehicleService.Domain.Repositories;

namespace VehicleService.Infrastructure.Persistence.Repositories
{
    public class VehicleRepository(VehicleDbContext dbContext) : IVehicleRepository
    {
        public async Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Vehicles
                                   .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
        }

        public async Task<Vehicle?> GetByVinAsync(string vin, CancellationToken cancellationToken = default)
        {
            var normalizedVin = vin.ToUpperInvariant();
            return await dbContext.Vehicles
                                   .FirstOrDefaultAsync(v => v.Vin == normalizedVin, cancellationToken);
        }

        public async Task<IEnumerable<Vehicle>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.Vehicles
                                   .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Vehicle>> ListAsync(Expression<Func<Vehicle, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await dbContext.Vehicles
                                   .Where(predicate)
                                   .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
        {
            await dbContext.Vehicles.AddAsync(vehicle, cancellationToken);
        }

        public void Delete(Vehicle vehicle)
        {
            dbContext.Vehicles.Remove(vehicle);
        }
    }
}