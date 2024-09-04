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

        // Add Booking
        [HttpPost("bookings")]
        public async Task<IActionResult> AddBooking([FromBody] BookingDto bookingDto)
        {
            await _servicerService.AddBookingAsync(bookingDto);
            return CreatedAtAction(nameof(GetBookingByIdAsync), new { bookingId = bookingDto.BookingId }, bookingDto);
        }


        // Get All Bookings for a Service
        [HttpGet("services/{serviceId}/bookings")]
        public async Task<ActionResult<IEnumerable<BookingResponseDto>>> GetBookingsAsync(int serviceId)
        {
            var bookings = await _servicerService.GetBookingsAsync(serviceId);
            return Ok(bookings);
        }

        // Get Booking by ID
        [HttpGet("bookings/{bookingId}")]
        public async Task<ActionResult<BookingResponseDto>> GetBookingByIdAsync(int bookingId)
        {
            var booking = await _servicerService.GetBookingByIdAsync(bookingId);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        // Remove Booking
        [HttpDelete("bookings/{bookingId}")]
        public async Task<IActionResult> RemoveBooking(int bookingId)
        {
            await _servicerService.RemoveBookingAsync(bookingId);
            return NoContent();
        }


        //Vehcile
        // Get Vehicles by ServiceInfoId
        [HttpGet("services/{serviceId}/vehicles")]
        public async Task<ActionResult<IEnumerable<VehicleDtoResponse>>> GetVehiclesAsync(int serviceId)
        {
            var vehicles = await _servicerService.GetVehiclesAsync(serviceId);
            return Ok(vehicles);
        }

        // Get Vehicle by ID
        [HttpGet("vehicles/{vehicleId}")]
        public async Task<ActionResult<VehicleDtoResponse>> GetVehicleByIdAsync(int vehicleId)
        {
            var vehicle = await _servicerService.GetVehicleByIdAsync(vehicleId);
            if (vehicle == null)
            {
                return NotFound();
            }
            return Ok(vehicle);
        }

        // Add Vehicle
        [HttpPost("vehicles")]
        public async Task<IActionResult> AddVehicle([FromBody] VehicleDto vehicleDto)
        {
            await _servicerService.AddVehicleAsync(vehicleDto);
            return CreatedAtAction(nameof(GetVehicleByIdAsync), new { vehicleId = vehicleDto.VehicleId }, vehicleDto);
        }

        // Remove Vehicle
        [HttpDelete("vehicles/{vehicleId}")]
        public async Task<IActionResult> RemoveVehicle(int vehicleId)
        {
            await _servicerService.RemoveVehicleAsync(vehicleId);
            return NoContent();
        }

        // Get Feedbacks by ServiceInfoId
        [HttpGet("services/{serviceId}/feedbacks")]
        public async Task<ActionResult<IEnumerable<FeedbackDtoResponse>>> GetFeedbacksAsync(int serviceId)
        {
            var feedbacks = await _servicerService.GetFeedbacksAsync(serviceId);
            return Ok(feedbacks);
        }
    }
}



