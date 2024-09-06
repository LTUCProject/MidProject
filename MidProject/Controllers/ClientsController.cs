using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidProject.Models;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;
using MidProject.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MidProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Policy = "ClientPolicy")] 
    public class ClientsController : ControllerBase
    {
        private readonly IClient _context;

        public ClientsController(IClient context)
        {
            _context = context;
        }

        // Session management
        [HttpGet("sessions")]
        public async Task<ActionResult<IEnumerable<Session>>> GetSessions(int clientId)
        {
            var result = await _context.GetClientSessionsAsync(clientId);
            return Ok(result);
        }

        [HttpGet("sessions/{sessionId}")]
        public async Task<ActionResult<Session>> GetSessionsById(int sessionId)
        {
            var result = await _context.GetSessionByIdAsync(sessionId);
            return Ok(result);
        }

        [HttpPost("sessions")]
        public async Task<ActionResult> StartSession([FromBody] SessionDto sessionDto)
        {
           var startedSession=  await _context.StartSessionAsync(sessionDto);
            //  return CreatedAtAction(nameof(GetSessionsById), new { sessionId = sessionDto.SessionId }, sessionDto);
            SessionDtoResponse response = new SessionDtoResponse()
            {
               SessionId= startedSession.SessionId,
                ClientId = startedSession.ClientId,
                ChargingStationId= startedSession.ChargingStationId,    
                StartTime= startedSession.StartTime,
                EndTime=startedSession.EndTime,
                EnergyConsumed=startedSession.EnergyConsumed,
            };
            return Ok(response);

        }

        [HttpPut("sessions/{sessionId}")]
        public async Task<ActionResult> EndSession(int sessionId)
        {
            await _context.EndSessionAsync(sessionId);
            return NoContent();
        }

        // Payment transactions management
        [HttpGet("PaymentTransaction/{sessionId}")]
        public async Task<ActionResult<IEnumerable<PaymentTransaction>>> GetClientPayment(int sessionId)
        {
            var result = await _context.GetClientPaymentsAsync(sessionId);
            return Ok(result);
        }

        [HttpPost("PaymentTransaction")]
        public async Task<ActionResult> AddPayment([FromBody] PaymentTransactionDto paymentDto)
        {
         var addedPatment=   await _context.AddPaymentAsync(paymentDto);
            //  return CreatedAtAction(nameof(GetClientPayment), new { sessionId = paymentDto.SessionId }, paymentDto);
            PaymentTransactionDtoResponse paymentTransactionDtoResponse = new PaymentTransactionDtoResponse()
            {
                PaymentTransactionId= addedPatment.PaymentTransactionId,
                SessionId= addedPatment.SessionId,
                ClientId= addedPatment.ClientId,
                Amount= addedPatment.Amount,
                PaymentDate= addedPatment.PaymentDate,
                PaymentMethod= addedPatment.PaymentMethod,
                Status= addedPatment.Status,
            };
            return Ok(paymentTransactionDtoResponse);
        }

        [HttpDelete("PaymentTransaction/{paymentId}")]
        public async Task<ActionResult> RemovePayment(int paymentId)
        {
            await _context.RemovePaymentAsync(paymentId);
            return NoContent();
        }

        // Favorites management
        [HttpGet("Favorite/{clientId}")]
        public async Task<ActionResult<IEnumerable<Favorite>>> GetClientFavorites(int clientId)
        {
            var result = await _context.GetClientFavoritesAsync(clientId);
            return Ok(result);
        }

        [HttpPost("Favorite")]
        public async Task<ActionResult> AddFavorite([FromBody] FavoriteDto favoriteDto)
        {
           var newFavorite= await _context.AddFavoriteAsync(favoriteDto);
            //  return CreatedAtAction(nameof(GetClientFavorites), new { clientId = favoriteDto.ClientId }, favoriteDto);
            FavoriteDtoResonse favoriteDtoResonse = new FavoriteDtoResonse()
            {
                FavoriteId= newFavorite.FavoriteId,
                ServiceInfoId= newFavorite.ServiceInfoId,
                ChargingStationId= newFavorite.ChargingStationId,
                ClientId=newFavorite.ClientId

            };
            return Ok(favoriteDtoResonse);
        }

        [HttpDelete("Favorite/{favoriteId}")]
        public async Task<ActionResult> RemoveFavorite(int favoriteId)
        {
            await _context.RemoveFavoriteAsync(favoriteId);
            return NoContent();
        }

        // Vehicle management
        [HttpGet("vehicles/{clientId}")]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetClientVehicles(int clientId)
        {
            var vehicles = await _context.GetClientVehiclesAsync(clientId);
            return Ok(vehicles);
        }

        [HttpGet("vehicle/{vehicleId}")]
        public async Task<ActionResult<Vehicle>> GetVehicleById(int vehicleId)
        {
            var vehicle = await _context.GetVehicleByIdAsync(vehicleId);
            return Ok(vehicle);
        }

        [HttpPost("vehicle")]
        public async Task<IActionResult> AddVehicle(VehicleDto vehicleDto)
        {
           

                var addedVehicle = await _context.AddVehicleAsync(vehicleDto);

                VehicleDtoResponse vehicleDtoResponse = new VehicleDtoResponse
                {
                    VehicleId = addedVehicle.VehicleId,           // Assuming AddVehicleAsync returns the entity
                    LicensePlate = addedVehicle.LicensePlate,
                    Model = addedVehicle.Model,
                    Year = addedVehicle.Year,
                    BatteryCapacity = addedVehicle.BatteryCapacity,
                    ElectricType = addedVehicle.ElectricType,
                    ClientId = addedVehicle.ClientId,
                    ServiceInfoId = addedVehicle.ServiceInfoId
                };
            return Ok(vehicleDtoResponse);
               
        }

        [HttpDelete("vehicles/{vehicleId}")]
        public async Task<ActionResult> RemoveVehicle(int vehicleId)
        {
            await _context.RemoveVehicleAsync(vehicleId);
            return NoContent();
        }

        // Booking management
        [HttpGet("bookings/{clientId}")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetClientBookings(int clientId)
        {
            var bookings = await _context.GetClientBookingsAsync(clientId);
            return Ok(bookings);
        }

        [HttpGet("booking/{bookingId}")]
        public async Task<ActionResult<Booking>> GetBookingById(int bookingId)
        {
            var booking = await _context.GetBookingByIdAsync(bookingId);
            if (booking == null)
                return NotFound();
            return Ok(booking);
        }

        [HttpPost("bookings")]
        public async Task<ActionResult> AddBooking([FromBody] BookingDto bookingDto)
        {
          var newBooking=  await _context.AddBookingAsync(bookingDto);
            // return CreatedAtAction(nameof(GetBookingById), new { bookingId = bookingDto.BookingId }, bookingDto);

            BookingResponseDto bookingResponseDto = new BookingResponseDto()
            {
                BookingId= newBooking.BookingId,
                ClientId=newBooking.ClientId,
                ServiceInfoId=newBooking.ServiceInfoId,
                VehicleId=newBooking.VehicleId,
                StartTime=newBooking.StartTime,
                EndTime=newBooking.EndTime,
                Status=newBooking.Status,
                Cost=newBooking.Cost

            };
            return Ok(bookingResponseDto);
        }

        [HttpDelete("bookings/{bookingId}")]
        public async Task<ActionResult> RemoveBooking(int bookingId)
        {
            await _context.RemoveBookingAsync(bookingId);
            return NoContent();
        }

        // Service request management
        [HttpGet("service-requests/{clientId}")]
        public async Task<ActionResult<IEnumerable<ServiceRequest>>> GetClientServiceRequests(int clientId)
        {
            var serviceRequests = await _context.GetClientServiceRequestsAsync(clientId);
            return Ok(serviceRequests);
        }

        [HttpGet("service-request/{requestId}")]
        public async Task<ActionResult<ServiceRequest>> GetServiceRequestById(int requestId)
        {
            var serviceRequest = await _context.GetServiceRequestByIdAsync(requestId);
            if (serviceRequest == null)
                return NotFound();
            return Ok(serviceRequest);
        }

        [HttpPost("service-requests")]
        public async Task<ActionResult> CreateServiceRequest([FromBody] ServiceRequestDto requestDto)
        {
         var newServiceReq=   await _context.CreateServiceRequestAsync(requestDto);
            //  return CreatedAtAction(nameof(GetServiceRequestById), new { requestId = requestDto.ServiceRequestId }, requestDto);
            ServiceRequestDtoResponse serviceRequestDtoResponse = new ServiceRequestDtoResponse()
            {
                ServiceRequestId = newServiceReq.ServiceRequestId,
                ServiceInfoId = newServiceReq.ServiceInfoId,
                ClientId = newServiceReq.ClientId,
                ProviderId = newServiceReq.ProviderId,
                Status  = newServiceReq.Status,
            };
            return Ok(serviceRequestDtoResponse);
        }

        [HttpDelete("service-requests/{requestId}")]
        public async Task<ActionResult> DeleteServiceRequest(int requestId)
        {
            await _context.DeleteServiceRequestAsync(requestId);
            return NoContent();
        }

        // Feedback management
        [HttpGet("feedbacks/{clientId}")]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetClientFeedbacks(int clientId)
        {
            var feedbacks = await _context.GetClientFeedbacksAsync(clientId);
            return Ok(feedbacks);
        }

        [HttpPost("feedbacks")]
        public async Task<ActionResult> AddFeedback([FromBody] FeedbackDto feedbackDto)
        {
          var newFeedback = await _context.AddFeedbackAsync(feedbackDto);
            // return CreatedAtAction(nameof(GetClientFeedbacks), new { clientId = feedbackDto.ClientId }, feedbackDto);

            FeedbackDtoResponse feedbackDtoResponse = new FeedbackDtoResponse()
            {
                FeedbackId= newFeedback.FeedbackId,
                ClientId= newFeedback.ClientId,
                ServiceInfoId= newFeedback.ServiceInfoId,
                Rating= newFeedback.Rating,
                Comments= newFeedback.Comments,
                Date    = newFeedback.Date,
            };
            return Ok(feedbackDtoResponse);
        }

        [HttpDelete("feedbacks/{feedbackId}")]
        public async Task<ActionResult> RemoveFeedback(int feedbackId)
        {
            await _context.RemoveFeedbackAsync(feedbackId);
            return NoContent();
        }

        // Notification management
        [HttpGet("notifications/{clientId}")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetClientNotifications(int clientId)
        {
            var notifications = await _context.GetClientNotificationsAsync(clientId);
            return Ok(notifications);
        }

        [HttpGet("notification/{notificationId}")]
        public async Task<ActionResult<Notification>> GetNotificationById(int notificationId)
        {
            var notification = await _context.GetNotificationByIdAsync(notificationId);
            if (notification == null)
                return NotFound();
            return Ok(notification);
        }

        
    }
}
