using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleService.Domain.Entities;

namespace VehicleService.Infrastructure.Persistence.Configurations
{
    public class VehicleEntityTypeConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.ToTable("Vehicles"); 

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Vin)
                .IsRequired()
                .HasMaxLength(17);
            builder.HasIndex(v => v.Vin).IsUnique();

            builder.Property(v => v.Manufacturer).IsRequired().HasMaxLength(100);
            builder.Property(v => v.Model).IsRequired().HasMaxLength(100);

            builder.Property(v => v.EngineType)
                .IsRequired()
                .HasConversion(v => v.ToString(), v => (Domain.Enums.EngineType)Enum.Parse(typeof(Domain.Enums.EngineType), v))
                .HasMaxLength(50);

            builder.Property(v => v.TransmissionType)
                .IsRequired()
                .HasConversion(v => v.ToString(), v => (Domain.Enums.TransmissionType)Enum.Parse(typeof(Domain.Enums.TransmissionType), v))
                .HasMaxLength(50);

            builder.Property(v => v.Status)
                .IsRequired()
                .HasConversion(v => v.ToString(), v => (Domain.Enums.VehicleStatus)Enum.Parse(typeof(Domain.Enums.VehicleStatus), v))
                .HasMaxLength(50);

            builder.OwnsOne(v => v.BasePrice, priceBuilder =>
            {

                priceBuilder.Property(p => p.Amount)
                    .HasColumnName("BasePrice_Amount")
                    .HasColumnType("numeric(19,4)") 
                    .IsRequired();

                priceBuilder.Property(p => p.Currency)
                    .HasColumnName("BasePrice_Currency")
                    .HasMaxLength(3)  
                    .IsRequired();
            });

            // builder.HasMany(v => v.Photos)
            //        .WithOne()
            //        .HasForeignKey(p => p.VehicleId)
            //        .OnDelete(DeleteBehavior.Cascade);

            // var navigation = builder.Metadata.FindNavigation(nameof(Vehicle.Photos));
            // navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}