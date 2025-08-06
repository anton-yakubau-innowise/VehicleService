using Microsoft.AspNetCore.Mvc;
using VehicleService.Application.Interfaces; 
using VehicleService.Application.Vehicles.Dtos;

namespace VehicleService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController(
        IVehicleApplicationService vehicleService,
        ILogger<VehiclesController> logger,
        ILogger<CustomControllerBase> baseLogger
    ) : CustomControllerBase(baseLogger)
    {

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVehicleById(Guid id)
        {
            logger.LogInformation("Attempting to get vehicle with ID: {id}", id);
            var vehicleDto = await vehicleService.GetVehicleByIdAsync(id);
            return HandleSingleResult(vehicleDto);
        }

        [HttpGet("vin/{vin}")]
        [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVehicleByVin(string vin)
        {
            logger.LogInformation("Attempting to get vehicle with VIN: {vin}", vin);
            if (string.IsNullOrWhiteSpace(vin))
            {
                logger.LogWarning("Attempted to get vehicle with empty VIN.");
                return BadRequest("VIN cannot be empty.");
            }
            
            var vehicleDto = await vehicleService.GetVehicleByVinAsync(vin);
            return HandleSingleResult(vehicleDto);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VehicleDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllVehicles()
        {
            logger.LogInformation("Attempting to get all vehicles.");
            var vehicles = await vehicleService.GetAllVehiclesAsync();

            logger.LogInformation("Retrieved {VehicleCount} vehicles.", vehicles.Count());
            return Ok(vehicles);
        }

        [HttpPost]
        [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleRequest request)
        {
            logger.LogInformation("Attempting to create a new vehicle.");
            if (request == null)
            {
                logger.LogWarning("Vehicle creation request is null.");
                return BadRequest("Vehicle creation request cannot be null.");
            }

            var createdVehicleDto = await vehicleService.CreateVehicleAsync(request);
            logger.LogInformation("Vehicle created with ID: {VehicleId}", createdVehicleDto.Id);
            return CreatedAtAction(nameof(GetVehicleById), new { id = createdVehicleDto.Id }, createdVehicleDto);

        }

        [HttpPatch("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchVehicle(Guid id, [FromBody] PatchVehicleRequest request)
        {
            logger.LogInformation("Attempting to patch vehicle with ID: {id}", id);
            if (request == null)
            {
                logger.LogWarning("Patch request body is null.");
                return BadRequest("Request body cannot be null.");
            }

            await vehicleService.PatchVehicleAsync(id, request);
            logger.LogInformation("Vehicle with ID: {id} patched successfully.", id);
            return NoContent();
        }

        [HttpPut("{id:guid}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetVehicleStatus(Guid id, [FromBody] SetVehicleStatusRequest request)
        {
            logger.LogInformation("Attempting to set status for vehicle with ID: {id}", id);
            if (request == null)
            {
                logger.LogWarning("SetVehicleStatus request body is null.");
                return BadRequest("Request body cannot be null.");
            }

            await vehicleService.SetVehicleStatusAsync(id, request.NewStatus);
            logger.LogInformation("Vehicle with ID: {id} status set to {NewStatus}.", id, request.NewStatus);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVehicle(Guid id)
        {
            logger.LogInformation("Attempting to delete vehicle with ID: {id}", id);
            await vehicleService.DeleteVehicleAsync(id);
            logger.LogInformation("Vehicle with ID: {id} deleted successfully.", id);
            return NoContent();
        }
    }
}
