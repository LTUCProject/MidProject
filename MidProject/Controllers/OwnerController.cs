using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidProject.Models.Dto.Request;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;
using MidProject.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MidProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "OwnerPolicy")]
    public class OwnerController : ControllerBase
    {
        private readonly IOwner _ownerService;

        public OwnerController(IOwner ownerService)
        {
            _ownerService = ownerService;
        }

        private string GetAccountId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Charging Station Management
        [HttpGet("chargingstations")]
        public async Task<ActionResult<IEnumerable<ChargingStationDtoResponse>>> GetAllChargingStations()
        {
            try
            {
                var accountId = GetAccountId();
                if (string.IsNullOrEmpty(accountId)) return Unauthorized("User is not authenticated.");

                var stations = await _ownerService.GetAllChargingStationsAsync(accountId);
                return Ok(stations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("chargingstations/{id}")]
        public async Task<ActionResult<ChargingStationDtoResponse>> GetChargingStationById(int id)
        {
            try
            {
                var stationDto = await _ownerService.GetChargingStationByIdAsync(id);
                if (stationDto == null) return NotFound();
                return Ok(stationDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("chargingstations")]
        public async Task<IActionResult> CreateChargingStation([FromBody] ChargingStationDtoRequest stationDtoRequest)
        {
            try
            {
                var accountId = GetAccountId();
                if (string.IsNullOrEmpty(accountId)) return Unauthorized("User is not authenticated.");

                var createdStation = await _ownerService.CreateChargingStationAsync(stationDtoRequest, accountId);
                return CreatedAtAction(nameof(GetChargingStationById), new { id = createdStation.ChargingStationId }, createdStation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("chargingstations/{id}")]
        public async Task<IActionResult> UpdateChargingStation(int id, [FromBody] ChargingStationDtoRequest stationDtoRequest)
        {
            try
            {
                if (id <= 0) return BadRequest("Invalid ID.");

                var accountId = GetAccountId();
                if (string.IsNullOrEmpty(accountId)) return Unauthorized("User is not authenticated.");

                await _ownerService.UpdateChargingStationAsync(id, stationDtoRequest, accountId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("chargingstations/{id}")]
        public async Task<IActionResult> DeleteChargingStation(int id)
        {
            try
            {
                var accountId = GetAccountId();
                if (string.IsNullOrEmpty(accountId)) return Unauthorized("User is not authenticated.");

                await _ownerService.DeleteChargingStationAsync(id, accountId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("stations/{stationId}/chargers")]
        public async Task<ActionResult<IEnumerable<ChargerDtoResponse>>> GetChargers(int stationId)
        {
            try
            {
                var chargers = await _ownerService.GetChargersAsync(stationId);
                return Ok(chargers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("chargers/{id}")]
        public async Task<ActionResult<ChargerDtoResponse>> GetChargerById(int id)
        {
            try
            {
                var charger = await _ownerService.GetChargerByIdAsync(id);
                if (charger == null) return NotFound();
                return Ok(charger);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("chargers")]
        public async Task<IActionResult> CreateCharger([FromBody] ChargerDtoRequest chargerDtoRequest)
        {
            try
            {
                await _ownerService.CreateChargerAsync(chargerDtoRequest);
                return CreatedAtAction(nameof(GetChargerById), new { id = chargerDtoRequest.ChargingStationId }, chargerDtoRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("chargers/{id}")]
        public async Task<IActionResult> UpdateCharger(int id, [FromBody] ChargerDtoRequest chargerDtoRequest)
        {
            try
            {
                if (id <= 0) return BadRequest("Invalid ID.");

                await _ownerService.UpdateChargerAsync(id, chargerDtoRequest);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("chargers/{id}")]
        public async Task<IActionResult> DeleteCharger(int id)
        {
            try
            {
                await _ownerService.DeleteChargerAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Maintenance Log Management
        [HttpGet("maintenance/{stationId}")]
        public async Task<ActionResult<IEnumerable<MaintenanceLogDtoResponse>>> GetMaintenanceLogs(int stationId)
        {
            try
            {
                var logs = await _ownerService.GetMaintenanceLogsAsync(stationId);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("maintenance")]
        public async Task<IActionResult> AddMaintenanceLog([FromBody] MaintenanceLogDtoRequest logDtoRequest)
        {
            try
            {
                await _ownerService.AddMaintenanceLogAsync(logDtoRequest);
                return CreatedAtAction(nameof(GetMaintenanceLogs), new { stationId = logDtoRequest.ChargingStationId }, logDtoRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("maintenance/{id}")]
        public async Task<IActionResult> RemoveMaintenanceLog(int id)
        {
            try
            {
                await _ownerService.RemoveMaintenanceLogAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}


//// Location Management
//[HttpGet("locations")]
//public async Task<ActionResult<IEnumerable<LocationDtoResponse>>> GetAllLocations()
//{
//    var locations = await _ownerService.GetAllLocationsAsync();
//    return Ok(locations);
//}

//[HttpGet("locations/{id}")]
//[Authorize(Policy = "OwnerPolicy")]
//public async Task<ActionResult<LocationDtoResponse>> GetLocationById(int id)
//{
//    var location = await _ownerService.GetLocationByIdAsync(id);
//    if (location == null) return NotFound();
//    return Ok(location);
//}

//[HttpPost("locations")]
//[Authorize(Policy = "OwnerPolicy")]
//public async Task<IActionResult> AddLocation([FromBody] LocationDtoRequest locationDtoRequest)
//{
//    await _ownerService.AddLocationAsync(locationDtoRequest);
//    // Assuming you want to return a 201 Created response
//    return CreatedAtAction(nameof(GetLocationById), new { id = locationDtoRequest.LocationId }, locationDtoRequest);
//}

//[HttpDelete("locations/{id}")]
//[Authorize(Policy = "OwnerPolicy")]
//public async Task<IActionResult> RemoveLocation(int id)
//{
//    await _ownerService.RemoveLocationAsync(id);
//    return NoContent();
//}