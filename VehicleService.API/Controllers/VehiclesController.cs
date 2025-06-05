using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleService.Application.Interfaces; 
using VehicleService.Application.Vehicles.Dtos;

namespace VehicleService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleApplicationService _vehicleService;

        public VehiclesController(IVehicleApplicationService vehicleService)
        {
            _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVehicleById(Guid id)
        {
            var vehicleDto = await _vehicleService.GetVehicleByIdAsync(id);
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
            var vehicleDto = await _vehicleService.GetVehicleByVinAsync(vin);
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
            var vehicles = await _vehicleService.GetAllVehiclesAsync();
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

            try
            {
                var createdVehicleDto = await _vehicleService.CreateVehicleAsync(request);
                return CreatedAtAction(nameof(GetVehicleById), new { id = createdVehicleDto.Id }, createdVehicleDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
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

            try
            {
                await _vehicleService.SetVehicleStatusAsync(id, request.NewStatus);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVehicle(Guid id)
        {
            try
            {
                await _vehicleService.DeleteVehicleAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                 return NotFound(ex.Message);
            }
        }
    }
}