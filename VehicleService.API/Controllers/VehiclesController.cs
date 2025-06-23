using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using VehicleService.Application.Interfaces; 
using VehicleService.Application.Vehicles.Dtos;

namespace VehicleService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController(IVehicleApplicationService vehicleService) : ControllerBase
    {
        
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVehicleById(Guid id)
        {
            var vehicleDto = await vehicleService.GetVehicleByIdAsync(id);
            if (vehicleDto == null)
            {
                return NotFound();
            }
            return Ok(vehicleDto);
        }

        [HttpGet("vin/{vin}")]
        [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVehicleByVin(string vin)
        {
            if (string.IsNullOrWhiteSpace(vin))
            {
                return BadRequest("VIN cannot be empty.");
            }
            
            var vehicleDto = await vehicleService.GetVehicleByVinAsync(vin);
            if (vehicleDto == null)
            {
                return NotFound();
            }
            return Ok(vehicleDto);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VehicleDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllVehicles()
        {
            var vehicles = await vehicleService.GetAllVehiclesAsync();
            return Ok(vehicles);
        }

        [HttpPost]
        [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleRequest request)
        {
            if (request == null)
            {
                return BadRequest("Vehicle creation request cannot be null.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdVehicleDto = await vehicleService.CreateVehicleAsync(request);
            return CreatedAtAction(nameof(GetVehicleById), new { id = createdVehicleDto.Id }, createdVehicleDto);
            
        }

        [HttpPatch("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchVehicle(Guid id, [FromBody] PatchVehicleRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request body cannot be null.");
            }

            await vehicleService.PatchVehicleAsync(id, request);
            return NoContent();
        }

        [HttpPut("{id:guid}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetVehicleStatus(Guid id, [FromBody] SetVehicleStatusRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request body cannot be null.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await vehicleService.SetVehicleStatusAsync(id, request.NewStatus);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVehicle(Guid id)
        {
            await vehicleService.DeleteVehicleAsync(id);
            return NoContent();
        }
    }
}