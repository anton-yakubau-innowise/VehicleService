using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleService.Domain.Entities;

public class VehiclePhotoEntityTypeConfiguration : IEntityTypeConfiguration<VehiclePhoto>
{
    public void Configure(EntityTypeBuilder<VehiclePhoto> builder)
    {
        builder.ToTable("VehiclePhoto");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();
        builder.Property(p => p.PhotoUrl).IsRequired().HasMaxLength(2048);
        builder.Property(p => p.Description).HasMaxLength(2000);
        builder.Property(p => p.IsPrimary).IsRequired();
        builder.Property(p => p.DisplayOrder).IsRequired();
        builder.Property(p => p.UploadedAt).IsRequired();
        builder.HasOne(p => p.Vehicle)
               .WithMany(v => v.Photos)
               .HasForeignKey(p => p.VehicleId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
