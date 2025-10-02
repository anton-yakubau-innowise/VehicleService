using VehicleService.Domain.Enums;
using VehicleService.Domain.Common;
using VehicleService.Domain.ValueObjects;

namespace VehicleService.Domain.Entities
{
    public class Vehicle
    {
        public Guid Id { get; private set; }
        public string Vin { get; private set; } = null!;
        public string Manufacturer { get; private set; } = null!;
        public string Model { get; private set; } = null!;
        public string Package { get; private set; } = null!;
        public string BodyType { get; private set; } = null!;
        public int Year { get; private set; }
        public string Color { get; private set; } = null!;
        public EngineType EngineType { get; private set; }
        public decimal EngineVolume { get; private set; }
        public int Power { get; private set; }
        public TransmissionType TransmissionType { get; private set; }
        public int Mileage { get; private set; }
        public Money BasePrice { get; private set; } = null!;
        public VehicleStatus Status { get; private set; }
        public string? Description { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public uint DbDataVersion { get; private set; }

        private readonly List<VehiclePhoto> _photos = new List<VehiclePhoto>();
        public IReadOnlyCollection<VehiclePhoto> Photos => _photos.AsReadOnly();

        private Vehicle()
        {
        }

        private Vehicle(
            string vin,
            string manufacturer,
            string model,
            string package,
            string bodyType,
            int year,
            string color,
            EngineType engineType,
            TransmissionType transmissionType,
            int mileage,
            Money basePrice)
        {
            Id = Guid.NewGuid();
            Vin = vin.ToUpperInvariant();
            Manufacturer = manufacturer;
            Model = model;
            Package = package;
            BodyType = bodyType;
            Year = year;
            Color = color;
            EngineType = engineType;
            TransmissionType = transmissionType;
            Mileage = mileage;
            BasePrice = basePrice;
            Status = VehicleStatus.Available;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public static Vehicle RegisterNewVehicle(
            string vin,
            string manufacturer,
            string model,
            string package,
            string bodyType,
            int year,
            string color,
            EngineType engineType,
            TransmissionType transmissionType,
            int initialMileage,
            decimal basePriceAmount,
            string basePriceCurrency)
        {

            Guard.AgainstNullOrWhiteSpace(vin);
            Guard.AgainstStringLength(vin, 17);
            Guard.AgainstNullOrWhiteSpace(manufacturer);
            Guard.AgainstNullOrWhiteSpace(model);
            Guard.AgainstNullOrWhiteSpace(package);
            Guard.AgainstNullOrWhiteSpace(bodyType);
            Guard.AgainstOutOfRange(year, 1886, DateTime.UtcNow.Year + 2);
            Guard.AgainstNullOrWhiteSpace(color);
            Guard.AgainstNegative(initialMileage);
            Guard.AgainstNegative(basePriceAmount);
            Guard.AgainstInvalidCurrencyCodeFormat(basePriceCurrency);

            var basePrice = new Money(basePriceAmount, basePriceCurrency);


            return new Vehicle(
                vin,
                manufacturer,
                model,
                package,
                bodyType,
                year,
                color,
                engineType,
                transmissionType,
                initialMileage,
                basePrice);
        }

        public void UpdateStatus(VehicleStatus newStatus)
        {
            Status = newStatus;
            SetUpdated();
        }

        public void SetReservedStatus()
        {
            if (Status != VehicleStatus.Available)
                throw new InvalidOperationException($"Cannot reserve vehicle with status {Status}. Vehicle must be Available.");
            Status = VehicleStatus.Reserved;
            SetUpdated();
        }

        public void SetSoldStatus()
        {
            if (Status != VehicleStatus.Available && Status != VehicleStatus.Reserved)
                throw new InvalidOperationException($"Cannot sell vehicle with status {Status}. Vehicle must be Available or Reserved.");
            Status = VehicleStatus.Sold;
            SetUpdated();
        }

        public void SetAvailableStatus()
        {
            if (Status == VehicleStatus.Sold)
                throw new InvalidOperationException("Cannot make a Sold vehicle Available without a specific return process.");
            Status = VehicleStatus.Available;
            SetUpdated();
        }

        public void UpdateBasePrice(Money newPrice)
        {
            Guard.AgainstNull(newPrice);
            Guard.AgainstNegative(newPrice.Amount);
            Guard.AgainstInvalidCurrencyCodeFormat(newPrice.Currency);
            BasePrice = newPrice;
            SetUpdated();
        }

        public void UpdateColor(string newColor)
        {
            Guard.AgainstNullOrWhiteSpace(newColor);
            Color = newColor;
            SetUpdated();
        }

        public void UpdateMileage(int newMileage)
        {
            Guard.AgainstNegative(newMileage);
            Mileage = newMileage;
            SetUpdated();
        }

        public void UpdateYear(int newYear)
        {
            Guard.AgainstOutOfRange(newYear, 1886, DateTime.UtcNow.Year + 2);
            Year = newYear;
            SetUpdated();
        }

        public void UpdateDetails(
            string? newColor = null,
            int? newYear = null,
            int? newMileage = null,
            Money? newBasePrice = null)
        {
            if (newColor is not null)
            {
                UpdateColor(newColor);
            }

            if (newYear is not null)
            {
                UpdateYear(newYear.Value);
            }

            if (newMileage is not null)
            {
                UpdateMileage(newMileage.Value);
            }

            if (newBasePrice is not null)
            {
                UpdateBasePrice(newBasePrice);
            }
        }
        
        public void AddPhoto(string photoUrl, string? description, bool isPrimary, int displayOrder)
        {
            Guard.AgainstNullOrWhiteSpace(photoUrl);

            var newVehiclePhoto = VehiclePhoto.Create(this.Id, photoUrl, description, isPrimary, displayOrder);

            if (isPrimary)
            {
                var currentPrimary = _photos.FirstOrDefault(p => p.IsPrimary);
                if (currentPrimary != null)
                {
                    currentPrimary.SetAsPrimary(false);
                }
            }

            _photos.Add(newVehiclePhoto);
            SetUpdated();
        }

        public void SetPrimaryPhoto(Guid photoId)
        {
            var photoToSetAsPrimary = _photos.FirstOrDefault(p => p.Id == photoId);
            if (photoToSetAsPrimary == null)
            {
                throw new InvalidOperationException("Photo to set as primary was not found in the vehicle's photo collection.");
            }

            var currentPrimary = _photos.FirstOrDefault(p => p.IsPrimary);
            if (currentPrimary != null)
            {
                currentPrimary.SetAsPrimary(false);
            }

            photoToSetAsPrimary.SetAsPrimary(true);

            SetUpdated();
        }

        public void RemovePhoto(Guid photoId)
        {
            var photoToRemove = _photos.FirstOrDefault(p => p.Id == photoId);
            if (photoToRemove != null)
            {
                _photos.Remove(photoToRemove);
                SetUpdated();
            }
        }

        private void ClearCurrentPrimaryPhoto()
        {
            var currentPrimary = _photos.FirstOrDefault(p => p.IsPrimary);
            if (currentPrimary != null)
            {
                currentPrimary.SetAsPrimary(false);
            }
        }

        private void SetUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
