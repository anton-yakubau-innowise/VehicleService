using Microsoft.AspNetCore.Mvc;
using VehicleService.Application.Vehicles.Dtos;

namespace VehicleService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class CustomControllerBase(ILogger<CustomControllerBase> logger) : ControllerBase
    {
        protected IActionResult HandleSingleResult<T>(T? result) where T : class
        {
            if (result is null)
            {
                logger.LogWarning("Resource of type {ResourceType} was not found.", typeof(T).Name);
                return NotFound();
            }

            return Ok(result);
        }
    }
}
