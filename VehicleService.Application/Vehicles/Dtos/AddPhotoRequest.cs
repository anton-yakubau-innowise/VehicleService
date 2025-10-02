using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace VehicleService.Application.Vehicles.Dtos
{
    public record AddPhotoRequest(
        [Required] IFormFile PhotoFile,
        string? Description,
        bool IsPrimary
    );
}