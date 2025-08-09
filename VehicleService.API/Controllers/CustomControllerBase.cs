using Microsoft.AspNetCore.Mvc;

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
                return NotFound();
            }

            return Ok(result);
        }
    }
}
