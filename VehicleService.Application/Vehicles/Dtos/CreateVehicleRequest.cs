using System.ComponentModel.DataAnnotations;
using VehicleService.Domain.Enums;

namespace VehicleService.Application.Vehicles.Dtos
{
    public record CreateVehicleRequest(
        [Required] string Vin,
        [Required] string Manufacturer,
        [Required] string Model,
        [Required] string Package,
        [Required] string BodyType,
        [Required] int Year,
        [Required] string Color,
        [Required] EngineType EngineType,
        [Required] TransmissionType TransmissionType,
        [Required] int InitialMileage,
        [Required] decimal BasePriceAmount,
        [Required] string BasePriceCurrency
    );
}