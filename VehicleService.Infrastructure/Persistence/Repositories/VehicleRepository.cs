using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using VehicleService.Domain.Entities;
using VehicleService.Domain.Repositories;
using VehicleService.Infrastructure.Persistence;

namespace VehicleService.Infrastructure.Persistence.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VehicleDbContext _dbContext;

        public VehicleRepository(VehicleDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Vehicles
                                   .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
        }

        public async Task<Vehicle?> GetByVinAsync(string vin, CancellationToken cancellationToken = default)
        {
            var normalizedVin = vin.ToUpperInvariant();
            return await _dbContext.Vehicles
                                   .FirstOrDefaultAsync(v => v.Vin == normalizedVin, cancellationToken);
        }

        public async Task<IEnumerable<Vehicle>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Vehicles
                                   .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Vehicle>> ListAsync(Expression<Func<Vehicle, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Vehicles
                                   .Where(predicate)
                                   .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
        {
            await _dbContext.Vehicles.AddAsync(vehicle, cancellationToken);
        }

        public void Update(Vehicle vehicle)
        {
            _dbContext.Entry(vehicle).State = EntityState.Modified;
        }

        public void Delete(Vehicle vehicle)
        {
            _dbContext.Vehicles.Remove(vehicle);
        }
    }
}