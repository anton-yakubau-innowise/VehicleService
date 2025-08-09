using Microsoft.AspNetCore.Mvc;
using VehicleService.Application.Interfaces; 
using VehicleService.Application.Vehicles.Dtos;
using VehicleService.Domain.Enums;

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
        public async Task<IActionResult> GetVehicleById(Guid id, CancellationToken cancellationToken)
        {
            var vehicleDto = await vehicleService.GetVehicleByIdAsync(id, cancellationToken);
            return HandleSingleResult(vehicleDto);
        }

        [HttpGet("vin/{vin}")]
        [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVehicleByVin(string vin, CancellationToken cancellationToken)
        {
            var vehicleDto = await vehicleService.GetVehicleByVinAsync(vin, cancellationToken);
            return HandleSingleResult(vehicleDto);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VehicleDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllVehicles(CancellationToken cancellationToken)
        {
            var vehicles = await vehicleService.GetAllVehiclesAsync(cancellationToken);
            return Ok(vehicles);
        }

        [HttpPost]
        [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return BadRequest("Vehicle creation request cannot be null.");
            }

            var createdVehicleDto = await vehicleService.CreateVehicleAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetVehicleById), new { id = createdVehicleDto.Id }, createdVehicleDto);

        }

        [HttpPatch("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchVehicle(Guid id, [FromBody] PatchVehicleRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return BadRequest("Request body cannot be null.");
            }

            await vehicleService.PatchVehicleAsync(id, request, cancellationToken);
            return NoContent();
        }

        [HttpPut("{id:guid}/status/sold")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetVehicleStatusToSold(Guid id, CancellationToken cancellationToken)
        {
            await vehicleService.SellVehicleAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpPut("{id:guid}/status/reserved")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetVehicleStatusToReserved(Guid id, CancellationToken cancellationToken)
        {
            await vehicleService.ReserveVehicleAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpPut("{id:guid}/status/available")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetVehicleStatusToAvailable(Guid id, CancellationToken cancellationToken)
        {
            await vehicleService.MakeVehicleAvailableAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVehicle(Guid id, CancellationToken cancellationToken)
        {
            await vehicleService.DeleteVehicleAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
