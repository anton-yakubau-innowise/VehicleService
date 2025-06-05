using VehicleService.Domain.Enums;

namespace VehicleService.Application.Vehicles.Dtos
{
    public record CreateVehicleRequest(
        string Vin,
        string Manufacturer,
        string Model,
        string Package,
        string BodyType,
        int Year,
        string Color,
        EngineType EngineType,
        TransmissionType TransmissionType,
        int InitialMileage,
        decimal BasePriceAmount,
        string BasePriceCurrency
    );
}