namespace VehicleService.Application.Vehicles.Dtos
{
    public record VehicleDto(
        Guid Id,
        string Vin,
        string Manufacturer,
        string Model,
        string Package,
        string BodyType,
        int Year,
        string Color,
        string EngineType,
        string TransmissionType,
        int Mileage,
        decimal BasePriceAmount,
        string BasePriceCurrency,
        string Status,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );
}