namespace VehicleService.Application.Vehicles.Dtos
{
    public record PatchVehicleRequest
    {
        public string? Manufacturer { get; init; }
        public string? Model { get; init; }
        public string? Package { get; init; }
        public string? Color { get; init; }
        public string? BodyType { get; init; }
        public int? Year { get; init; }
        public string? EngineType { get; init; }
        public string? TransmissionType { get; init; }
        public int? Mileage { get; init; }
        public decimal? BasePriceAmount { get; init; }
        public string? BasePriceCurrency { get; init; }
    }
}