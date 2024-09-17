using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;
using MidProject.Repository.Interfaces;
using MidProject.Repository.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MidProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ClientPolicy")]
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
            var startedSession = await _context.StartSessionAsync(sessionDto);
            //  return CreatedAtAction(nameof(GetSessionsById), new { sessionId = sessionDto.SessionId }, sessionDto);
            SessionDtoResponse response = new SessionDtoResponse()
            {
                SessionId = startedSession.SessionId,
                ClientId = startedSession.ClientId,
                ChargingStationId = startedSession.ChargingStationId,
                StartTime = startedSession.StartTime,
                EndTime = startedSession.EndTime,
                EnergyConsumed = startedSession.EnergyConsumed,
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
            var addedPatment = await _context.AddPaymentAsync(paymentDto);
            //  return CreatedAtAction(nameof(GetClientPayment), new { sessionId = paymentDto.SessionId }, paymentDto);
            PaymentTransactionDtoResponse paymentTransactionDtoResponse = new PaymentTransactionDtoResponse()
            {
                PaymentTransactionId = addedPatment.PaymentTransactionId,
                SessionId = addedPatment.SessionId,
                ClientId = addedPatment.ClientId,
                Amount = addedPatment.Amount,
                PaymentDate = addedPatment.PaymentDate,
                PaymentMethod = addedPatment.PaymentMethod,
                Status = addedPatment.Status,
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
        // Retrieve all charging station favorites for a client
        [HttpGet("ChargingStationsFavorites/{clientId}")]
        public async Task<ActionResult<IEnumerable<FavoriteChargingStationResponseDto>>> GetClientChargingStationFavorites(int clientId)
        {
            var result = await _context.GetClientChargingStationFavoritesAsync(clientId);
            return Ok(result);
        }

        // Retrieve all service info favorites for a client
        [HttpGet("ServiceInfosFavorites/{clientId}")]
        public async Task<ActionResult<IEnumerable<FavoriteServiceInfoResponseDto>>> GetClientServiceInfoFavorites(int clientId)
        {
            var result = await _context.GetClientServiceInfoFavoritesAsync(clientId);
            return Ok(result);
        }

        // Add a new charging station favorite
        [HttpPost("ChargingStationFavorites")]
        public async Task<ActionResult<FavoriteChargingStationResponseDto>> AddChargingStationFavorite([FromBody] FavoriteChargingStationDto favoriteDto)
        {
            var newFavorite = await _context.AddChargingStationFavoriteAsync(favoriteDto);
            return CreatedAtAction(nameof(GetClientChargingStationFavorites), new { clientId = favoriteDto.ClientId }, newFavorite);
        }

        // Add a new service info favorite
        [HttpPost("ServiceInfoFavorites")]
        public async Task<ActionResult<FavoriteServiceInfoResponseDto>> AddServiceInfoFavorite([FromBody] FavoriteServiceInfoDto favoriteDto)
        {
            var newFavorite = await _context.AddServiceInfoFavoriteAsync(favoriteDto);
            return CreatedAtAction(nameof(GetClientServiceInfoFavorites), new { clientId = favoriteDto.ClientId }, newFavorite);
        }

        // Remove a charging station favorite
        [HttpDelete("ChargingStationFavorites/{favoriteId}")]
        public async Task<IActionResult> RemoveChargingStationFavorite(int favoriteId)
        {
            await _context.RemoveChargingStationFavoriteAsync(favoriteId);
            return NoContent();
        }

        // Remove a service info favorite
        [HttpDelete("ServiceInfoFavorites/{favoriteId}")]
        public async Task<IActionResult> RemoveServiceInfoFavorite(int favoriteId)
        {
            await _context.RemoveServiceInfoFavoriteAsync(favoriteId);
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
        public async Task<IActionResult> AddVehicle([FromBody] VehicleDto vehicleDto)
        {
            try
            {
                if (vehicleDto == null)
                {
                    return BadRequest("Vehicle data is required.");
                }

                var addedVehicle = await _context.AddVehicleAsync(vehicleDto);

                VehicleDtoResponse vehicleDtoResponse = new VehicleDtoResponse
                {
                    VehicleId = addedVehicle.VehicleId,
                    LicensePlate = addedVehicle.LicensePlate,
                    Model = addedVehicle.Model,
                    Year = addedVehicle.Year,
                    BatteryCapacity = addedVehicle.BatteryCapacity,
                    ElectricType = addedVehicle.ElectricType,
                    ClientId = addedVehicle.ClientId
                   
                };

                return Ok(vehicleDtoResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding the vehicle: {ex.Message}");
            }
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
            var newBooking = await _context.AddBookingAsync(bookingDto);
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
        public async Task<ActionResult> CreateServiceRequest([FromBody] ClientServiceRequestDto requestDto)
        {
            var newServiceReq = await _context.CreateServiceRequestAsync(requestDto);

            ServiceRequestDtoResponse serviceRequestDtoResponse = new ServiceRequestDtoResponse()
            {
                ServiceRequestId = newServiceReq.ServiceRequestId,
                ServiceInfoId = newServiceReq.ServiceInfoId,
                ClientId = newServiceReq.ClientId,
                ProviderId = newServiceReq.ProviderId,
                VehicleId = newServiceReq.VehicleId, // Added vehicle information to the response
                Status = newServiceReq.Status,
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
                FeedbackId = newFeedback.FeedbackId,
                ClientId = newFeedback.ClientId,
                ServiceInfoId = newFeedback.ServiceInfoId,
                Rating = newFeedback.Rating,
                Comments = newFeedback.Comments,
                Date = newFeedback.Date,
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

        // Post management
        [HttpGet("posts")]
        public async Task<ActionResult<IEnumerable<PostResponseDto>>> GetAllPosts()
        {
            // Retrieve all posts from the database
            var posts = await _context.GetAllPostsAsync();

            // Map posts to PostResponseDto
            var postDtos = posts.Select(p => new PostResponseDto
            {
                PostId = p.PostId,
                AccountId = p.AccountId, // Use AccountId
                UserName = p.Account != null ? p.Account.UserName : "Unknown", // Use UserName from Account
                Title = p.Title,
                Content = p.Content,
                Date = p.Date,
                Comments = p.Comments.Select(c => new CommentResponseDto
                {
                    CommentId = c.CommentId,
                    AccountId = c.AccountId, // Use AccountId
                    PostId = c.PostId,
                    Content = c.Content,
                    Date = c.Date,
                    UserName = c.Account != null ? c.Account.UserName : "Unknown" // Map UserName if needed
                }).ToList() // Ensure comments collection is materialized
            }).ToList(); // Materialize the posts collection with ToList

            return Ok(postDtos);
        }





        [HttpPost("posts")]
        public async Task<ActionResult<PostResponseDto>> AddPost([FromBody] PostDto postDto)
        {
            var postResponse = await _context.AddPostAsync(postDto);
            return CreatedAtAction(nameof(GetAllPosts), new { postId = postResponse.PostId }, postResponse);
        }

        // UpdatePostById Controller Method
        [HttpPut("Posts/{id}")]
        public async Task<IActionResult> UpdatePostById(int id, [FromBody] PostDto postDto)
        {
            var updatedPost = await _context.UpdatePostByIdAsync(id, postDto);

            if (updatedPost == null)
            {
                return NotFound(); // Handle case where post is not found
            }

            return Ok(updatedPost);
        }


        [HttpDelete("posts/{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            await _context.DeletePostAsync(postId);
            return NoContent();
        }

        // Comment management
        [HttpGet("comments")]
        public async Task<ActionResult<IEnumerable<CommentResponseDto>>> GetAllComments()
        {
            var commentDtos = await _context.GetAllCommentsAsync();
            return Ok(commentDtos);
        }



        [HttpPost("comments")]
        public async Task<ActionResult<CommentResponseDto>> AddComment([FromBody] CommentDto commentDto)
        {
            // Call the service method to add the comment and get the response DTO
            CommentResponseDto commentResponseDto = await _context.AddCommentAsync(commentDto);

            // Return the response DTO with UserName populated
            return Ok(commentResponseDto);
        }

        [HttpPut("Comments/{id}")]
        public async Task<IActionResult> UpdateCommentById(int id, [FromBody] CommentDto commentDto)
        {
            var updatedComment = await _context.UpdateCommentByIdAsync(id, commentDto);

            if (updatedComment == null)
            {
                return NotFound(); // Handle case where comment is not found
            }

            return Ok(updatedComment);
        }

        [HttpDelete("comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            await _context.DeleteCommentAsync(commentId);
            return NoContent();
        }
    }
}