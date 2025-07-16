using VehicleService.Domain.Enums;
using VehicleService.Domain.Common;
using VehicleService.Domain.ValueObjects;

namespace VehicleService.Domain.Entities
{
    public class Vehicle
    {
        public Guid Id { get; private set; }
        public string Vin { get; private set; }
        public string Manufacturer { get; private set; }
        public string Model { get; private set; }
        public string Package { get; private set; }
        public string BodyType { get; private set; }
        public int Year { get; private set; }
        public string Color { get; private set; }
        public EngineType EngineType { get; private set; }
        public TransmissionType TransmissionType { get; private set; }
        public int Mileage { get; private set; }
        public Money BasePrice { get; private set; }
        public VehicleStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        private Vehicle()
        {
            Vin = string.Empty;
            Manufacturer = string.Empty;
            Model = string.Empty;
            Package = string.Empty;
            BodyType = string.Empty;
            Color = string.Empty;
            BasePrice = null!;
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
            
            
            return new Vehicle()
            {
                Id = Guid.NewGuid(),
                Vin = vin.ToUpperInvariant(),
                Manufacturer = manufacturer,
                Model = model,
                Package = package,
                BodyType = bodyType,
                Year = year,
                Color = color,
                EngineType = engineType,
                TransmissionType = transmissionType,
                Mileage = initialMileage,
                BasePrice = basePrice,
                Status = VehicleStatus.Available,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public void UpdateStatus(VehicleStatus newStatus)
        {
            Status = newStatus;
        }

        public void SetReservedStatus()
        {
            if (Status != VehicleStatus.Available)
                throw new InvalidOperationException($"Cannot reserve vehicle with status {Status}. Vehicle must be Available.");
            Status = VehicleStatus.Reserved;
        }

        public void SetSoldStatus()
        {
            if (Status != VehicleStatus.Available && Status != VehicleStatus.Reserved)
                throw new InvalidOperationException($"Cannot sell vehicle with status {Status}. Vehicle must be Available or Reserved.");
            Status = VehicleStatus.Sold;
        }

        public void SetAvailableStatus()
        {
            if (Status == VehicleStatus.Sold)
                throw new InvalidOperationException("Cannot make a Sold vehicle Available without a specific return process.");
            Status = VehicleStatus.Available;
        }

        public void UpdateBasePrice(Money newPrice)
        {
            BasePrice = newPrice;
        }
        
        public void UpdateColor(string newColor)
        {
            Guard.AgainstNullOrWhiteSpace(newColor);
            Color = newColor;
        }

        public void UpdateMileage(int newMileage)
        {
            Guard.AgainstNegative(newMileage);
            Mileage = newMileage;
        }

        public void UpdateYear(int newYear)
        {
            Guard.AgainstOutOfRange(newYear, 1886, DateTime.UtcNow.Year + 2);
            Year = newYear;
        }
    }
}