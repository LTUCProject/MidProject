using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;
using MidProject.Repository.Interfaces;
using MidProject.Repository.Services;
using System.Collections.Generic;
using System.Security.Claims;
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

        private string GetAccountId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // or another claim type for accountId
        }

        [HttpGet("ChargingStations")]
        public async Task<IActionResult> GetAllChargingStations()
        {
            var chargingStations = await _context.GetAllChargingStationsAsync();
            return Ok(chargingStations);
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
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost("sessions")]
        public async Task<ActionResult<SessionDtoResponse>> StartSession([FromBody] SessionDto sessionDto)
        {
            var startedSession = await _context.StartSessionAsync(sessionDto);

            var response = new SessionDtoResponse
            {
                SessionId = startedSession.SessionId,
                ClientId = startedSession.ClientId,
                ChargingStationId = startedSession.ChargingStationId,
                StartTime = startedSession.StartTime,
                EndTime = startedSession.EndTime,
                EnergyConsumed = startedSession.EnergyConsumed
            };

            return CreatedAtAction(nameof(GetSessionsById), new { sessionId = startedSession.SessionId }, response);
        }

        [HttpPut("sessions/{sessionId}")]
        public async Task<IActionResult> EndSession(int sessionId)
        {
            await _context.EndSessionAsync(sessionId);
            return NoContent();
        }

        // Payment transactions management
        [HttpGet("sessions/{sessionId}/payments")]
        public async Task<ActionResult<IEnumerable<PaymentTransaction>>> GetClientPayments(int sessionId)
        {
            var result = await _context.GetClientPaymentsAsync(sessionId);
            return Ok(result);
        }

        [HttpPost("sessions/{sessionId}/payments")]
        public async Task<ActionResult<PaymentTransactionDtoResponse>> AddPayment([FromBody] PaymentTransactionDto paymentDto)
        {
            var addedPayment = await _context.AddPaymentAsync(paymentDto);

            var response = new PaymentTransactionDtoResponse
            {
                PaymentTransactionId = addedPayment.PaymentTransactionId,
                SessionId = addedPayment.SessionId,
                ClientId = addedPayment.ClientId,
                Amount = addedPayment.Amount,
                PaymentDate = addedPayment.PaymentDate,
                PaymentMethod = addedPayment.PaymentMethod,
                Status = addedPayment.Status
            };

            return CreatedAtAction(nameof(GetClientPayments), new { sessionId = addedPayment.SessionId }, response);
        }

        [HttpDelete("payments/{paymentId}")]
        public async Task<IActionResult> RemovePayment(int paymentId)
        {
            await _context.RemovePaymentAsync(paymentId);
            return NoContent();
        }


        // Favorites management
        // Retrieve all charging station favorites for a client
        [HttpGet("ChargingStationsFavorites")]
        public async Task<ActionResult<IEnumerable<FavoriteChargingStationResponseDto>>> GetClientChargingStationFavorites()
        {
            var result = await _context.GetClientChargingStationFavoritesAsync();
            return Ok(result);
        }


        // Retrieve all service info favorites for a client
        [HttpGet("ServiceInfosFavorites/{clientId}")]
        public async Task<ActionResult<IEnumerable<FavoriteServiceInfoResponseDto>>> GetClientServiceInfoFavorites()
        {
            var result = await _context.GetClientServiceInfoFavoritesAsync();
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
        [HttpGet("vehicle")]
        public async Task<ActionResult<IEnumerable<VehicleResponseDto>>> GetClientVehicles()
        {
            // Retrieve account ID of the logged-in user
            var accountId = GetAccountId(); // Assuming GetAccountId() returns the string accountId

            if (string.IsNullOrEmpty(accountId))
            {
                return Unauthorized("Invalid account ID in token.");
            }

            try
            {
                var vehicles = await _context.GetClientVehiclesAsync(accountId);
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                // Log the error if necessary
                return StatusCode(500, "An error occurred while retrieving vehicles.");
            }
        }

        [HttpGet("vehicle/{vehicleId}")]
        public async Task<ActionResult<Vehicle>> GetVehicleById(int vehicleId)
        {
            var vehicle = await _context.GetVehicleByIdAsync(vehicleId);
            return Ok(vehicle);
        }

        [HttpPost("add-vehicle")]
        public async Task<ActionResult<Vehicle>> AddVehicle(VehicleDto vehicleDto)
        {
            var accountId = GetAccountId(); // Retrieve the account ID from the token

            if (string.IsNullOrEmpty(accountId))
            {
                return Unauthorized("Invalid account ID in token.");
            }

            try
            {
                var vehicle = await _context.AddVehicleAsync(vehicleDto, accountId);
                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the vehicle.");
            }
        }


        [HttpDelete("vehicles/{vehicleId}")]
        public async Task<ActionResult> RemoveVehicle(int vehicleId)
        {
            await _context.RemoveVehicleAsync(vehicleId);
            return NoContent();
        }


        // Booking management

        // Retrieve bookings for a specific client
        [HttpGet("bookings/{clientId}")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetClientBookings()
        {
            var bookings = await _context.GetClientBookingsAsync();
            if (bookings == null || !bookings.Any())
                return NotFound("No bookings found for this client.");
            return Ok(bookings);
        }

        // Retrieve a specific booking by its ID
        [HttpGet("booking/{bookingId}")]
        public async Task<ActionResult<Booking>> GetBookingById(int bookingId)
        {
            var booking = await _context.GetBookingByIdAsync(bookingId);
            if (booking == null)
                return NotFound("Booking not found.");
            return Ok(booking);
        }

        // Create a new booking
        [HttpPost("bookings")]
        public async Task<ActionResult<BookingResponseDto>> AddBooking([FromBody] ClientBookingDto bookingDto)
        {
            // Ensure the bookingDto contains valid data
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newBooking = await _context.AddBookingAsync(bookingDto);

            if (newBooking == null)
                return BadRequest("Error creating booking.");

            var bookingResponseDto = new BookingResponseDto()
            {
                BookingId = newBooking.BookingId,
                ClientId = newBooking.ClientId,
                ChargingStationId = newBooking.ChargingStationId, // Updated field
                VehicleId = newBooking.VehicleId,
                StartTime = newBooking.StartTime,
                EndTime = newBooking.EndTime,
                Status = newBooking.Status,
                Cost = newBooking.Cost
            };

            return CreatedAtAction(nameof(GetBookingById), new { bookingId = bookingResponseDto.BookingId }, bookingResponseDto);
        }

        // Remove a specific booking by its ID
        [HttpDelete("bookings/{bookingId}")]
        public async Task<ActionResult> RemoveBooking(int bookingId)
        {
            var bookingExists = await _context.GetBookingByIdAsync(bookingId);
            if (bookingExists == null)
                return NotFound("Booking not found.");



            await _context.RemoveBookingAsync(bookingId);
            return NoContent();
        }


        // Get all service requests for a client
        [HttpGet("service-requests")]
        public async Task<ActionResult<IEnumerable<ServiceRequest>>> GetClientServiceRequests()
        {
            try
            {
                var serviceRequests = await _context.GetClientServiceRequestsAsync();
                return Ok(serviceRequests);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Client not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // Get service request by ID
        [HttpGet("service-request/{requestId}")]
        public async Task<ActionResult<ServiceRequest>> GetServiceRequestById(int requestId)
        {
            var serviceRequest = await _context.GetServiceRequestByIdAsync(requestId);
            if (serviceRequest == null)
                return NotFound("Service request not found");
            return Ok(serviceRequest);
        }

        // Get all service infos
        [HttpGet("service-infos")]
        public async Task<ActionResult<IEnumerable<ServiceInfoResponseDto>>> GetAllServiceInfos()
        {
            try
            {
                var serviceInfos = await _context.GetAllServiceInfosAsync();
                return Ok(serviceInfos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // Create a new service request
        [HttpPost("service-requests")]
        public async Task<ActionResult<ServiceRequestDtoResponse>> CreateServiceRequest([FromBody] ClientServiceRequestDto requestDto)
        {
            try
            {
                var newServiceReq = await _context.CreateServiceRequestAsync(requestDto);

                var serviceRequestDtoResponse = new ServiceRequestDtoResponse
                {
                    ServiceRequestId = newServiceReq.ServiceRequestId,
                    ServiceInfoId = newServiceReq.ServiceInfoId,
                    ClientId = newServiceReq.ClientId,
                    ProviderId = newServiceReq.ProviderId,
                    VehicleId = newServiceReq.VehicleId, // Include vehicle information in the response
                    Status = newServiceReq.Status
                };

                return Ok(serviceRequestDtoResponse);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Client not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // Delete a service request by ID
        [HttpDelete("service-requests/{requestId}")]
        public async Task<ActionResult> DeleteServiceRequest(int requestId)
        {
            try
            {
                await _context.DeleteServiceRequestAsync(requestId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
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
        public async Task<ActionResult<IEnumerable<NotResDto>>> GetClientNotifications()
        {
            var notifications = await _context.GetClientNotificationsAsync();

            // Map Notifications to NotResDto
            var notResDtos = notifications.Select(n => new NotResDto
            {
                NotificationId = n.NotificationId,
                ClientId = n.ClientId,
                Title = n.Title,
                Message = n.Message,
                Date = n.Date
            });

            return Ok(notResDtos);
        }

        [HttpGet("notification/{notificationId}")]
        public async Task<ActionResult<NotResDto>> GetNotificationById(int notificationId)
        {
            var notification = await _context.GetNotificationByIdAsync(notificationId);
            if (notification == null)
                return NotFound();

            // Map Notification to NotResDto
            var notResDto = new NotResDto
            {
                NotificationId = notification.NotificationId,
                ClientId = notification.ClientId,
                Title = notification.Title,
                Message = notification.Message,
                Date = notification.Date
            };

            return Ok(notResDto);
        }


        [HttpDelete("notification/{notificationId}")]
        public async Task<IActionResult> DeleteNotification(int notificationId)
        {
            try
            {
                await _context.DeleteNotificationAsync(notificationId);
                return NoContent(); // 204 No Content response indicating successful deletion
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("You are not authorized to delete this notification.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Notification not found or does not belong to the client.");
            }
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