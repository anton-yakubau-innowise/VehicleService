using VehicleService.Application.Interfaces;
using VehicleService.Domain.Repositories;
using VehicleService.Infrastructure.Persistence.Repositories;

namespace VehicleService.Infrastructure.Persistence
{
public class UnitOfWork : IUnitOfWork
{
    private readonly VehicleDbContext _dbContext;
    private IVehicleRepository? _vehicles;

    public UnitOfWork(VehicleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IVehicleRepository Vehicles => _vehicles ??= new VehicleRepository(_dbContext);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}
}