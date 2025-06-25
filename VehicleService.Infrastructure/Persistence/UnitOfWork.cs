using VehicleService.Application.Interfaces;
using VehicleService.Domain.Repositories;
using VehicleService.Infrastructure.Persistence.Repositories;

namespace VehicleService.Infrastructure.Persistence
{
public class UnitOfWork(VehicleDbContext dbContext) : IUnitOfWork
{
    private IVehicleRepository? vehicles;

    public IVehicleRepository Vehicles => vehicles ??= new VehicleRepository(dbContext);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}
}