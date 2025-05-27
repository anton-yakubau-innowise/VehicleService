using System;
using VehicleService.Domain.Common;

namespace VehicleService.Domain.Entities
{
    public class VehiclePhoto
    {
        public Guid Id { get; private set; }
        public Guid VehicleId { get; private set; }
        public string PhotoUrl { get; private set; } 
        public string? Description { get; private set; }
        public bool IsPrimary { get; private set; }
        public int DisplayOrder { get; private set; }
        public DateTime UploadedAt { get; private set; }

        private VehiclePhoto()
        {
            PhotoUrl = string.Empty;
        }

        private VehiclePhoto(Guid id, Guid vehicleId, string photoUrl, string? description, bool isPrimary, int displayOrder)
        {
            Guard.AgainstEmptyGuid(id);
            Guard.AgainstEmptyGuid(vehicleId);
            Guard.AgainstNullOrWhiteSpace(photoUrl);

            Id = id;
            VehicleId = vehicleId;
            PhotoUrl = photoUrl;
            Description = description;
            IsPrimary = isPrimary;
            DisplayOrder = displayOrder;
            UploadedAt = DateTime.UtcNow;
        }

        public static VehiclePhoto AddPhoto
        (Guid vehicleId, string photoUrl, string? description, bool isPrimary, int displayOrder)
        {
            return new VehiclePhoto(Guid.NewGuid(), vehicleId, photoUrl, description, isPrimary, displayOrder);
        }

        public void SetAsPrimary(bool isPrimary)
        {
            IsPrimary = isPrimary;
        }

        public void UpdateDisplayOrder(int newOrder)
        {
            DisplayOrder = newOrder;
        }
        
        public void UpdateDescription(string? newDescription)
        {
            Description = newDescription;
        }
    }
}