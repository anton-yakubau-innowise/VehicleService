using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VehicleService.Domain.Entities;

namespace VehicleService.Infrastructure.Persistence
{
    public class VehicleDbContext : DbContext
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehiclePhoto> VehiclePhotos { get; set; } = null!;

        public VehicleDbContext(DbContextOptions<VehicleDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}