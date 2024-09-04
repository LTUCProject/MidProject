using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidProject.Models.Dto.Request;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;
using MidProject.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MidProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "ServicerPolicy")] 
    public class ServicerController : ControllerBase
    {
        private readonly IServicer _servicerService;

        public ServicerController(IServicer servicerService)
        {
            _servicerService = servicerService;
        }

        // Add Service Request
        [HttpPost("servicerequests")]
        public async Task<IActionResult> AddServiceRequest([FromBody] ServiceRequestDtoRequest requestDto)
        {
            await _servicerService.AddServiceRequestAsync(requestDto);
            return CreatedAtAction(nameof(GetServiceRequestsAsync), new { serviceId = requestDto.ServiceInfoId }, requestDto);
        }

        // Get Service Requests
        [HttpGet("services/{serviceId}/servicerequests")]
        public async Task<ActionResult<IEnumerable<ServiceRequestDtoResponse>>> GetServiceRequestsAsync(int serviceId)
        {
            var requests = await _servicerService.GetServiceRequestsAsync(serviceId);
            return Ok(requests);
        }

        // Update Service
        [HttpPut("services/{serviceId}")]
        public async Task<IActionResult> UpdateService(int serviceId, [FromBody] ServiceInfoDtoRequest serviceDto)
        {
            if (serviceDto.ServiceInfoId != serviceId)
            {
                return BadRequest();
            }
            await _servicerService.UpdateServiceAsync(serviceDto);
            return NoContent();
        }

        // Create Service
        [HttpPost("services")]
        public async Task<IActionResult> CreateService([FromBody] ServiceInfoDtoRequest serviceDto)
        {
            await _servicerService.CreateServiceAsync(serviceDto);
            return CreatedAtAction(nameof(GetServiceByIdAsync), new { serviceId = serviceDto.ServiceInfoId }, serviceDto);
        }

        // Get Service by ID
        [HttpGet("services/{serviceId}")]
        public async Task<ActionResult<ServiceInfoDtoResponse>> GetServiceByIdAsync(int serviceId)
        {
            var service = await _servicerService.GetServiceByIdAsync(serviceId);
            if (service == null)
            {
                return NotFound();
            }
            return Ok(service);
        }

        // Delete Service
        [HttpDelete("services/{serviceId}")]
        public async Task<IActionResult> DeleteService(int serviceId)
        {
            await _servicerService.DeleteServiceAsync(serviceId);
            return NoContent();
        }
    }
}


//// Add Feedback
//[HttpPost("feedbacks")]
//public async Task<IActionResult> AddFeedback([FromBody] Feedback feedback)
//{
//    await _servicerService.AddFeedbackAsync(feedback);
//    return CreatedAtAction(nameof(GetFeedbacksAsync), new { serviceId = feedback.ServiceInfoId }, feedback);
//}

//// Get Feedbacks
//[HttpGet("services/{serviceId}/feedbacks")]
//public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacksAsync(int serviceId)
//{
//    var feedbacks = await _servicerService.GetFeedbacksAsync(serviceId);
//    return Ok(feedbacks);
//}



//// Add Vehicle
//[HttpPost("vehicles")]
//public async Task<IActionResult> AddVehicle([FromBody] Vehicle vehicle)
//{
//    await _servicerService.AddVehicleAsync(vehicle);
//    return CreatedAtAction(nameof(GetVehicleByIdAsync), new { vehicleId = vehicle.VehicleId }, vehicle);
//}

//// Get Vehicle by ID
//[HttpGet("vehicles/{vehicleId}")]
//public async Task<ActionResult<Vehicle>> GetVehicleByIdAsync(int vehicleId)
//{
//    var vehicle = await _servicerService.GetVehicleByIdAsync(vehicleId);
//    if (vehicle == null)
//    {
//        return NotFound();
//    }
//    return Ok(vehicle);
//}

//// Remove Vehicle
//[HttpDelete("vehicles/{vehicleId}")]
//public async Task<IActionResult> RemoveVehicle(int vehicleId)
//{
//    await _servicerService.RemoveVehicleAsync(vehicleId);
//    return NoContent();
//}