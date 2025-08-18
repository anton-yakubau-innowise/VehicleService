using Grpc.Core;
using VehicleService.Domain.Repositories;
using VehicleService.GRPC;

namespace VehicleService.API.Services;

public class VehicleGrpcService(ILogger<VehicleGrpcService> logger, IVehicleRepository vehicleRepository) : VehicleApi.VehicleApiBase
{
    public override async Task<VehicleDetailsResponse> GetVehicleDetails(
    GetVehicleDetailsRequest request, ServerCallContext context)
    {
        logger.LogInformation("gRPC request for vehicle {VehicleId}", request.VehicleId);

        if (!Guid.TryParse(request.VehicleId, out var vehicleGuid))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid GUID format"));
        }

        var vehicle = await vehicleRepository.GetByIdAsync(vehicleGuid);

        if (vehicle == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Vehicle with id {request.VehicleId} not found"));
        }
        
        logger.LogInformation("Vehicle found: {VehicleId} - {Model}", vehicleGuid, vehicle.Model);
        return new VehicleDetailsResponse
        {
            Id = vehicleGuid.ToString(),
            Model = vehicle.Model,
            Price = vehicle.BasePrice.Amount.ToString(System.Globalization.CultureInfo.InvariantCulture),
            Currency = vehicle.BasePrice.Currency.ToString(),
        };
    }
}