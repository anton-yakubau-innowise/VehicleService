using System.ComponentModel.DataAnnotations;
using VehicleService.Domain.Enums;

namespace VehicleService.Application.Vehicles.Dtos
{
    public record SetVehicleStatusRequest([Required] VehicleStatus NewStatus);
}