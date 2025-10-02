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
        decimal EngineVolume,
        int Power,
        string TransmissionType,
        int Mileage,
        decimal BasePriceAmount,
        string BasePriceCurrency,
        string Description,
        string Status,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        List<VehiclePhotoDto> Photos
    );
}
