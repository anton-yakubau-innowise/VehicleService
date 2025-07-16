using VehicleService.Domain.Enums;

namespace VehicleService.Application.Vehicles.Dtos
{
    public record SetVehicleStatusRequest(VehicleStatus NewStatus);
}