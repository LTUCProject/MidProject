using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MidProject.Models;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;
using MidProject.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MidProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
  //  [Authorize(Policy = "ServicerPolicy")]
    public class ServicerController : ControllerBase
    {
        private readonly IServicer _servicerService;

        public ServicerController(IServicer servicerService)
        {
            _servicerService = servicerService;
        }

        // Add Service Request
        [HttpPost("servicerequests")]
        public async Task<IActionResult> AddServiceRequest([FromBody] ServiceRequestDto requestDto)
        {
            try
            {
                await _servicerService.AddServiceRequestAsync(requestDto);
                return CreatedAtAction(nameof(GetServiceRequestsAsync), new { serviceId = requestDto.ServiceInfoId }, requestDto);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error adding service request: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // Get Service Requests
        [HttpGet("services/{serviceId}/servicerequests")]
        public async Task<ActionResult<IEnumerable<ServiceRequestDtoResponse>>> GetServiceRequestsAsync(int serviceId)
        {
            try
            {
                var requests = await _servicerService.GetServiceRequestsAsync(serviceId);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error fetching service requests: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // Update Service
        [HttpPut("services/{serviceId}")]
        public async Task<IActionResult> UpdateService(int serviceId, [FromBody] ServiceInfoDto serviceDto)
        {
            try
            {
                if (serviceDto.ServiceInfoId != serviceId)
                {
                    return BadRequest("Service ID mismatch");
                }

                await _servicerService.UpdateServiceAsync(serviceDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error updating service: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // Create Service
        [HttpPost("serviceinfo")]
        public async Task<IActionResult> AddServiceInfo([FromBody] ServiceInfoDto serviceInfoDto)
        {
            if (serviceInfoDto == null)
            {
                return BadRequest("ServiceInfoDto is null.");
            }

            // Validate the DTO here if needed

            await _servicerService.AddServiceInfoAsync(serviceInfoDto);

            return Ok("ServiceInfo created successfully.");
        }

        // Get Service by ID
        [HttpGet("services/{serviceId}")]
        public async Task<ActionResult<ServiceInfoDtoResponse>> GetServiceByIdAsync(int serviceId)
        {
            try
            {
                var service = await _servicerService.GetServiceByIdAsync(serviceId);
                if (service == null)
                {
                    return NotFound();
                }
                return Ok(service);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error fetching service by ID: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // Delete Service
        [HttpDelete("services/{serviceId}")]
        public async Task<IActionResult> DeleteService(int serviceId)
        {
            try
            {
                await _servicerService.DeleteServiceAsync(serviceId);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error deleting service: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // Add Booking
        [HttpPost("bookings")]
        public async Task<IActionResult> AddBooking([FromBody] BookingDto bookingDto)
        {
            var newBooking = await _servicerService.AddBookingAsync(bookingDto);
            // return CreatedAtAction(nameof(GetBookingById), new { bookingId = bookingDto.BookingId }, bookingDto);

            BookingResponseDto bookingResponseDto = new BookingResponseDto()
            {
                BookingId = newBooking.BookingId,
                ClientId = newBooking.ClientId,
                ServiceInfoId = newBooking.ServiceInfoId,
                VehicleId = newBooking.VehicleId,
                StartTime = newBooking.StartTime,
                EndTime = newBooking.EndTime,
                Status = newBooking.Status,
                Cost = newBooking.Cost

            };
            return Ok(bookingResponseDto);
        }

        // Get All Bookings for a Service
        [HttpGet("services/{serviceId}/bookings")]
        public async Task<ActionResult<IEnumerable<BookingResponseDto>>> GetBookingsAsync(int serviceId)
        {
            try
            {
                var bookings = await _servicerService.GetBookingsAsync(serviceId);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error fetching bookings: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // Get Booking by ID
        [HttpGet("bookings/{bookingId}")]
        public async Task<ActionResult<BookingResponseDto>> GetBookingByIdAsync(int bookingId)
        {
            try
            {
                var booking = await _servicerService.GetBookingByIdAsync(bookingId);
                if (booking == null)
                {
                    return NotFound();
                }
                return Ok(booking);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error fetching booking by ID: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // Remove Booking
        [HttpDelete("bookings/{bookingId}")]
        public async Task<IActionResult> RemoveBooking(int bookingId)
        {
            try
            {
                await _servicerService.RemoveBookingAsync(bookingId);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error removing booking: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // Get Vehicles by ServiceInfoId
        [HttpGet("services/{serviceId}/vehicles")]
        public async Task<ActionResult<IEnumerable<VehicleDtoResponse>>> GetVehiclesAsync(int serviceId)
        {
            try
            {
                var vehicles = await _servicerService.GetVehiclesAsync(serviceId);
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error fetching vehicles: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // Get Vehicle by ID
        [HttpGet("vehicles/{vehicleId}")]
        public async Task<ActionResult<VehicleDtoResponse>> GetVehicleByIdAsync(int vehicleId)
        {
            try
            {
                var vehicle = await _servicerService.GetVehicleByIdAsync(vehicleId);
                if (vehicle == null)
                {
                    return NotFound();
                }
                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error fetching vehicle by ID: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // Add Vehicle
        [HttpPost("vehicles")]
        public async Task<IActionResult> AddVehicle([FromBody] VehicleDto vehicleDto)
        {
            try
            {
                await _servicerService.AddVehicleAsync(vehicleDto);
                //  return CreatedAtAction(nameof(GetVehicleByIdAsync), new { vehicleId = vehicleDto.VehicleId }, vehicleDto);

                var x = new Vehicle
                {
                    LicensePlate= vehicleDto.LicensePlate,

                };
                return Ok(x);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error adding vehicle: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // Remove Vehicle
        [HttpDelete("vehicles/{vehicleId}")]
        public async Task<IActionResult> RemoveVehicle(int vehicleId)
        {
            try
            {
                await _servicerService.RemoveVehicleAsync(vehicleId);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error removing vehicle: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // Get Feedbacks by ServiceInfoId
        [HttpGet("services/{serviceId}/feedbacks")]
        public async Task<ActionResult<IEnumerable<FeedbackDtoResponse>>> GetFeedbacksAsync(int serviceId)
        {
            try
            {
                var feedbacks = await _servicerService.GetFeedbacksAsync(serviceId);
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error fetching feedbacks: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
