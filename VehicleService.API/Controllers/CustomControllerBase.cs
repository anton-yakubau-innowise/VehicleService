using Microsoft.AspNetCore.Mvc;

namespace VehicleService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class CustomControllerBase : ControllerBase
    {
        protected IActionResult HandleVehicleDto<VehicleDto>(VehicleDto? vehicleDto) 
        {
            if (vehicleDto is null)
            {
                return NotFound();
            }
            return Ok(vehicleDto);
        }
    }
}