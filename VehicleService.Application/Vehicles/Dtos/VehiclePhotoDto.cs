namespace VehicleService.Application.Vehicles.Dtos;

public record VehiclePhotoDto(
    Guid Id,
    string PhotoUrl,
    string? Description,
    bool IsPrimary,
    int DisplayOrder
);


