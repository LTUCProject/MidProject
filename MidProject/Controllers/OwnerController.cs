using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidProject.Models;
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
        [HttpGet("sessions/{stationId}")]
        [Authorize(Policy = "OwnerPolicy")]
        public async Task<IActionResult> GetSessionsByChargingStationAsync(int stationId)
        {
            // Extract the account ID from the claims (assuming it's stored in the claims)
            var accountId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(accountId))
            {
                return Unauthorized("User is not authenticated.");
            }

            try
            {
                var sessions = await _ownerService.GetSessionsByChargingStationAsync(stationId, accountId);

                if (sessions == null)
                {
                    return NotFound("Charging station or sessions not found.");
                }

                return Ok(sessions);
            }
            catch (Exception ex)
            {
                // Log exception details (ex) here as needed
                return StatusCode(500, "Internal server error.");
            }
        }
        private string GetAccountId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Charging Station Management
        [HttpGet("chargingstations")]
        public async Task<ActionResult<IEnumerable<ChargingStationResponseDto>>> GetAllChargingStations()
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
        public async Task<ActionResult<ChargingStationResponseDto>> GetChargingStationById(int id)
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
        public async Task<IActionResult> CreateChargingStation([FromBody] ChargingStationDto stationDtoRequest)
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
        public async Task<IActionResult> UpdateChargingStation(int id, [FromBody] ChargingStationDto stationDtoRequest)
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
        public async Task<ActionResult<IEnumerable<ChargerResponseDto>>> GetChargers(int stationId)
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
        public async Task<ActionResult<ChargerResponseDto>> GetChargerById(int id)
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
        public async Task<IActionResult> CreateCharger([FromBody] ChargerDto chargerDtoRequest)
        {

            var newCharger = await _ownerService.CreateChargerAsync(chargerDtoRequest);
            //  return CreatedAtAction(nameof(GetChargerById), new { id = chargerDtoRequest.ChargingStationId }, chargerDtoRequest);

            ChargerResponseDto chargerResponseDto = new ChargerResponseDto()
            {
                ChargerId = newCharger.ChargerId,
                Type = newCharger.Type,
                Power = newCharger.Power,
                Speed = newCharger.Speed,
                ChargingStationId = newCharger.ChargingStationId,
            };
            return Ok(chargerResponseDto);
        }

        [HttpPut("chargers/{id}")]
        public async Task<IActionResult> UpdateCharger(int id, [FromBody] ChargerDto chargerDtoRequest)
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
        public async Task<IActionResult> AddMaintenanceLog([FromBody] MaintenanceLogDto logDtoRequest)
        {

            var newLog = await _ownerService.AddMaintenanceLogAsync(logDtoRequest);
            // return CreatedAtAction(nameof(GetMaintenanceLogs), new { stationId = logDtoRequest.ChargingStationId }, logDtoRequest);

            MaintenanceLogDtoResponse response = new MaintenanceLogDtoResponse()
            {
                MaintenanceLogId = newLog.MaintenanceLogId,
                ChargingStationId = newLog.ChargingStationId,
                MaintenanceDate = newLog.MaintenanceDate,
                PerformedBy = newLog.PerformedBy,
                Details = newLog.Details,
                Cost = newLog.Cost
            };
            return Ok(response);

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

        // Notification Management
        [HttpPost("OwnerNotifications")]
        public async Task<ActionResult<NotificationResponseDto>> CreateNotificationAsync([FromBody] NotificationDto notificationDto)
        {
            var notification = await _ownerService.CreateNotificationAsync(notificationDto);

            // Use CreatedAtRoute instead of CreatedAtAction
            return CreatedAtRoute(
                "OwnerGetNotificationById", // Name the route
                new { notificationId = notification.NotificationId },
                notification
            );
        }

        [HttpGet("OwnerNotifications/{notificationId}", Name = "OwnerGetNotificationById")]
        public async Task<ActionResult<NotificationResponseDto>> GetNotificationByIdAsync(int notificationId)
        {
            var notification = await _ownerService.GetNotificationByIdAsync(notificationId);
            if (notification == null)
            {
                return NotFound();
            }
            return Ok(notification);
        }



        [HttpGet("OwnerNotifications/client/{clientId}")]
        public async Task<ActionResult<IEnumerable<NotificationResponseDto>>> GetNotificationsByClientIdAsync(int clientId)
        {
            var notifications = await _ownerService.GetNotificationsByClientIdAsync(clientId);
            return Ok(notifications);
        }

        // GET: api/location
        [HttpGet("locations")]
        public async Task<ActionResult<IEnumerable<LocationResponseDto>>> GetAllLocations()
        {
            try
            {
                var locations = await _ownerService.GetAllLocationsAsync();
                return Ok(locations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/location/{id}
        [HttpGet("locations/{id}")]
        public async Task<ActionResult<LocationResponseDto>> GetLocationById(int id)
        {
            try
            {
                var locationDto = await _ownerService.GetLocationByIdAsync(id);
                if (locationDto == null) return NotFound();
                return Ok(locationDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/location
        [HttpPost("locations")]
        public async Task<IActionResult> CreateLocation([FromBody] LocationDto locationDtoRequest)
        {
            try
            {
                var createdLocation = await _ownerService.CreateLocationAsync(locationDtoRequest);
                return CreatedAtAction(nameof(GetLocationById), new { id = createdLocation.LocationId }, createdLocation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/location/{id}
        [HttpPut("locations/{id}")]
        public async Task<IActionResult> UpdateLocation(int id, [FromBody] LocationDto locationDtoRequest)
        {
            try
            {
                if (id <= 0) return BadRequest("Invalid ID.");

                await _ownerService.UpdateLocationAsync(id, locationDtoRequest);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/location/{id}
        [HttpDelete("locations/{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            try
            {
                await _ownerService.DeleteLocationAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("posts")]
        public async Task<ActionResult<IEnumerable<PostResponseDto>>> GetAllPosts()
        {
            // Retrieve all posts from the database
            var posts = await _ownerService.GetAllPostsAsync();

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
            var postResponse = await _ownerService.AddPostAsync(postDto);
            return CreatedAtAction(nameof(GetAllPosts), new { postId = postResponse.PostId }, postResponse);
        }

        // UpdatePostById Controller Method
        [HttpPut("Posts/{id}")]
        public async Task<IActionResult> UpdatePostById(int id, [FromBody] PostDto postDto)
        {
            var updatedPost = await _ownerService.UpdatePostByIdAsync(id, postDto);

            if (updatedPost == null)
            {
                return NotFound(); // Handle case where post is not found
            }

            return Ok(updatedPost);
        }


        [HttpDelete("posts/{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            await _ownerService.DeletePostAsync(postId);
            return NoContent();
        }

        // Comment management
        [HttpGet("comments")]
        public async Task<ActionResult<IEnumerable<CommentResponseDto>>> GetAllComments()
        {
            var commentDtos = await _ownerService.GetAllCommentsAsync();
            return Ok(commentDtos);
        }



        [HttpPost("comments")]
        public async Task<ActionResult<CommentResponseDto>> AddComment([FromBody] CommentDto commentDto)
        {
            // Call the service method to add the comment and get the response DTO
            CommentResponseDto commentResponseDto = await _ownerService.AddCommentAsync(commentDto);

            // Return the response DTO with UserName populated
            return Ok(commentResponseDto);
        }


        [HttpPut("Comments/{id}")]
        public async Task<IActionResult> UpdateCommentById(int id, [FromBody] CommentDto commentDto)
        {
            var updatedComment = await _ownerService.UpdateCommentByIdAsync(id, commentDto);

            if (updatedComment == null)
            {
                return NotFound(); // Handle case where comment is not found
            }

            return Ok(updatedComment);
        }


        [HttpDelete("comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            await _ownerService.DeleteCommentAsync(commentId);
            return NoContent();
        }

    }

}


