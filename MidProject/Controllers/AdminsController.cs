using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MidProject.Data;
using MidProject.Models;
using MidProject.Repository.Interfaces;
using MidProject.Repository.Services;
using MidProject.Repository.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MidProject.Models.Dto.Response;
using MidProject.Models.Dto.Request2;
using Microsoft.AspNetCore.Authorization;
using MidProject.Models.Dto.Request;

namespace MidProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]
    public class AdminsController : ControllerBase
    {
        private readonly MidprojectDbContext _context;
        private readonly IAdmin _adminService;

        public AdminsController(MidprojectDbContext context, IAdmin adminService)
        {
            _context = context;
            _adminService = adminService;
        }

        // Start Client management ================================================================================================================
        // GET: api/Admins/Clients
        [HttpGet("Clients")]
        public async Task<ActionResult<IEnumerable<ClientResponseDto>>> GetAllClients()
        {
            var clients = await _adminService.GetAllClientsAsync();
            return Ok(clients);
        }

        // GET: api/Admins/Clients/5
        [HttpGet("Clients/{id}")]
        public async Task<ActionResult<ClientResponseDto>> GetClientById(int id)
        {
            var client = await _adminService.GetClientByIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        // DELETE: api/Admins/Clients/5
        [HttpDelete("Clients/{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _adminService.GetClientByIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            await _adminService.DeleteClientAsync(id);
            return NoContent();
        }
        // End Client management ================================================================================================================

        // Start Provider management ================================================================================================================
        // GET: api/Admin/providers
        [HttpGet("providers")]
        public async Task<ActionResult<IEnumerable<ProviderResponseDto>>> GetAllProviders()
        {
            var providers = await _adminService.GetAllProvidersAsync();
            var providerDtos = providers.Select(p => new ProviderResponseDto
            {
                ProviderId = p.ProviderId,
                Name = p.Name,
                Email = p.Email,
                Type = p.Type
            }).ToList();

            return Ok(providerDtos);
        }

        // GET: api/Admin/providers/{id}
        [HttpGet("providers/{id}")]
        public async Task<ActionResult<ProviderResponseDto>> GetProviderById(int id)
        {
            var provider = await _adminService.GetProviderByIdAsync(id);

            if (provider == null)
            {
                return NotFound();
            }

            var providerDto = new ProviderResponseDto
            {
                ProviderId = provider.ProviderId,
                Name = provider.Name,
                Email = provider.Email,
                Type = provider.Type
            };

            return Ok(providerDto);
        }

        // DELETE: api/Admin/providers/{id}
        [HttpDelete("providers/{id}")]
        public async Task<IActionResult> DeleteProvider(int id)
        {
            await _adminService.DeleteProviderAsync(id);
            return NoContent();
        }
        // End Provider management ================================================================================================================

        // Start Client subscriptions management ================================================================================================
        // GET: api/Admins/Clients/{clientId}/Subscriptions
        [HttpGet("Clients/{clientId}/Subscriptions")]
        public async Task<IActionResult> GetClientSubscriptions(int clientId)
        {
            var subscriptions = await _adminService.GetClientSubscriptionByIdAsync(clientId);
            if (subscriptions == null)
            {
                return NotFound($"No subscriptions found for client with ID {clientId}.");
            }

            return Ok(subscriptions);
        }

        // DELETE: api/Admins/Subscriptions/{clientSubscriptionId}
        [HttpDelete("Subscriptions/{clientSubscriptionId}")]
        public async Task<IActionResult> RemoveClientSubscription(int clientSubscriptionId)
        {
            var subscription = await _adminService.GetClientSubscriptionByIdAsync(clientSubscriptionId);

            if (subscription == null)
            {
                return NotFound($"Subscription with ID {clientSubscriptionId} not found.");
            }

            await _adminService.RemoveClientSubscriptionAsync(clientSubscriptionId);
            return NoContent();
        }
        // End Client subscriptions management ================================================================================================

        // Start Subscription plan management ================================================================================================
        // GET: api/Admin/SubscriptionPlans
        [HttpGet("SubscriptionPlans")]
        public async Task<ActionResult<IEnumerable<SubscriptionPlanResponseDto>>> GetAllSubscriptionPlans()
        {
            var subscriptionPlans = await _adminService.GetAllSubscriptionPlansAsync();
            return Ok(subscriptionPlans);
        }

        // GET: api/Admin/SubscriptionPlans/{id}
        [HttpGet("SubscriptionPlans/{id}")]
        public async Task<ActionResult<SubscriptionPlanResponseDto>> GetSubscriptionPlanById(int id)
        {
            var subscriptionPlan = await _adminService.GetSubscriptionPlanByIdAsync(id);

            if (subscriptionPlan == null)
            {
                return NotFound();
            }

            return Ok(subscriptionPlan);
        }
        [HttpPost("SubscriptionPlans")]
        public async Task<IActionResult> CreateSubscriptionPlan([FromBody] SubscriptionPlanDto newSubscriptionPlanDto)
        {
            if (newSubscriptionPlanDto == null)
            {
                return BadRequest("Invalid subscription plan data.");
            }

            
                var createdPlan = await _adminService.CreateSubscriptionPlanAsync(newSubscriptionPlanDto);
            //     return CreatedAtAction(nameof(GetSubscriptionPlanById), new { id = createdPlan.Id }, createdPlan);

            SubscriptionPlanResponseDto subscriptionPlanDtoResonse = new SubscriptionPlanResponseDto()
            {
                SubscriptionPlanId= createdPlan.SubscriptionPlanId,
                Name= createdPlan.Name,
                Description= createdPlan.Description,
                Price= createdPlan.Price,
                DurationInDays= createdPlan.DurationInDays,
            };
            return Ok(subscriptionPlanDtoResonse);
            
        }

        // DELETE: api/Admin/SubscriptionPlans/{id}
        [HttpDelete("SubscriptionPlans/{id}")]
        public async Task<IActionResult> DeleteSubscriptionPlan(int id)
        {
            var subscriptionPlan = await _adminService.GetSubscriptionPlanByIdAsync(id);

            if (subscriptionPlan == null)
            {
                return NotFound();
            }

            await _adminService.DeleteSubscriptionPlanAsync(id);
            return NoContent();
        }

        // GET: api/Admin/ClientSubscriptions
        [HttpGet("ClientSubscriptions")]
        public async Task<ActionResult<IEnumerable<ClientSubscriptionResponseDto>>> GetAllClientSubscriptions()
        {
            try
            {
                var clientSubscriptions = await _adminService.GetAllClientSubscriptionsAsync();

                if (clientSubscriptions == null || !clientSubscriptions.Any())
                {
                    return NotFound("No client subscriptions found.");
                }

                return Ok(clientSubscriptions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // End Subscription plan management ================================================================================================

        // Start Notifications management ================================================================================================

        // GET: api/Admin/Notifications
        [HttpGet("Notifications")]
        public async Task<IActionResult> GetAdminNotifications()
        {
            try
            {
                var notifications = await _adminService.GetAdminNotificationsAsync();
                return Ok(notifications);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        // POST: api/Admin/Notifications
        [HttpPost("Notifications")]
        public async Task<IActionResult> AddNotificationForAllClients([FromBody] NotificationDto notificationDto)
        {
            if (notificationDto == null)
            {
                return BadRequest("Notification data is required.");
            }

            await _adminService.AddNotificationForAllClientsAsync(notificationDto);
            return Ok(new { message = "Notification sent to all clients successfully." });
        }

        // DELETE: api/Admin/Notifications/{notificationId}
        [HttpDelete("Notifications/{notificationId}")]
        public async Task<IActionResult> DeleteNotification(int notificationId)
        {
            try
            {
                await _adminService.DeleteNotificationAsync(notificationId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // End Notifications management ================================================================================================


        // Start Charging station management ================================================================================================

        [HttpGet("ChargingStations")]
        public async Task<IActionResult> GetAllChargingStations()
        {
            var chargingStations = await _adminService.GetAllChargingStationsAsync();
            return Ok(chargingStations);
        }

        [HttpGet("ChargingStations/{id}")]
        public async Task<IActionResult> GetChargingStationById(int id)
        {
            var chargingStation = await _adminService.GetChargingStationByIdAsync(id);
            if (chargingStation == null)
            {
                return NotFound();
            }
            return Ok(chargingStation);
        }

        [HttpDelete("ChargingStations/{id}")]
        public async Task<IActionResult> DeleteChargingStation(int id)
        {
            var chargingStation = await _adminService.GetChargingStationByIdAsync(id);
            if (chargingStation == null)
            {
                return NotFound();
            }

            await _adminService.DeleteChargingStationAsync(id);
            return NoContent();
        }
        // End Charging station management ================================================================================================

        // Start Vehicle management ================================================================================================
        [HttpGet("Vehicles")]
        public async Task<IActionResult> GetAllVehicles()
        {
            var vehicles = await _adminService.GetAllVehiclesAsync();
            var vehicleDtos = vehicles.Select(v => new VehicleResponseDto
            {
                VehicleId = v.VehicleId,
                LicensePlate = v.LicensePlate,
                Model = v.Model,
                Year = v.Year,
                BatteryCapacity = v.BatteryCapacity,
                ElectricType = v.ElectricType,
                ClientId = v.ClientId
            });
            return Ok(vehicleDtos);
        }

        [HttpGet("Vehicles/{id}")]
        public async Task<IActionResult> GetVehicleById(int id)
        {
            var vehicle = await _adminService.GetVehicleByIdAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            var vehicleDto = new VehicleResponseDto
            {
                VehicleId = vehicle.VehicleId,
                LicensePlate = vehicle.LicensePlate,
                Model = vehicle.Model,
                Year = vehicle.Year,
                BatteryCapacity = vehicle.BatteryCapacity,
                ElectricType = vehicle.ElectricType,
                ClientId = vehicle.ClientId
            };
            return Ok(vehicleDto);
        }

        [HttpDelete("Vehicles/{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await _adminService.GetVehicleByIdAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            await _adminService.DeleteVehicleAsync(id);
            return NoContent();
        }
        // End Vehicle management ================================================================================================

       
        //// Start Location management ================================================================================================
        //[HttpGet("Locations")]
        //public async Task<IActionResult> GetAllLocations()
        //{
        //    var locations = await _adminService.GetAllLocationsAsync();
        //    var locationDtos = locations.Select(l => new LocationResponseDto
        //    {
        //        LocationId = l.LocationId,
        //        Name = l.Name,
        //        Address = l.Address,
        //        Latitude = l.Latitude,
        //        Longitude = l.Longitude,
        //        ChargingStations = l.ChargingStations.Select(cs => new ChargingStationResponseDto
        //        {
        //            ChargingStationId = cs.ChargingStationId,
        //            StationLocation = cs.StationLocation,
        //            LocationId = l.LocationId, // Map LocationId here if needed
        //            Name = cs.Name,
        //            HasParking = cs.HasParking,
        //            Status = cs.Status,
        //            PaymentMethod = cs.PaymentMethod
        //        }).ToList()
        //    }).ToList();
        //    return Ok(locationDtos);
        //}


        //[HttpGet("Locations/{id}")]
        //public async Task<IActionResult> GetLocationById(int id)
        //{
        //    var location = await _adminService.GetLocationByIdAsync(id);
        //    if (location == null)
        //    {
        //        return NotFound();
        //    }
        //    var locationDto = new LocationResponseDto
        //    {
        //        LocationId = location.LocationId,
        //        Name = location.Name,
        //        Address = location.Address,
        //        Latitude = location.Latitude,
        //        Longitude = location.Longitude,
        //        ChargingStations = location.ChargingStations.Select(cs => new ChargingStationResponseDto
        //        {
        //            ChargingStationId = cs.ChargingStationId,
        //            StationLocation = cs.StationLocation,
        //            LocationId = location.LocationId, // Map LocationId here if needed
        //            Name = cs.Name,
        //            HasParking = cs.HasParking,
        //            Status = cs.Status,
        //            PaymentMethod = cs.PaymentMethod
        //        }).ToList()
        //    };
        //    return Ok(locationDto);
        //}


        //[HttpPost("Locations")]
        //public async Task<IActionResult> CreateLocation([FromBody] LocationDto locationDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var location = new Location
        //    {
        //        Name = locationDto.Name,
        //        Address = locationDto.Address,
        //        Latitude = locationDto.Latitude,
        //        Longitude = locationDto.Longitude
        //    };

        //    await _adminService.CreateLocationAsync(location);
        //    return CreatedAtAction(nameof(GetLocationById), new { id = location.LocationId }, location);
        //}

        //[HttpPut("Locations/{id}")]
        //public async Task<IActionResult> UpdateLocation(int id, [FromBody] LocationDto locationDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var location = await _adminService.GetLocationByIdAsync(id);
        //    if (location == null)
        //    {
        //        return NotFound();
        //    }

        //    location.Name = locationDto.Name;
        //    location.Address = locationDto.Address;
        //    location.Latitude = locationDto.Latitude;
        //    location.Longitude = locationDto.Longitude;

        //    await _adminService.UpdateLocationAsync(location);
        //    return NoContent();
        //}

        //[HttpDelete("Locations/{id}")]
        //public async Task<IActionResult> DeleteLocation(int id)
        //{
        //    var location = await _adminService.GetLocationByIdAsync(id);
        //    if (location == null)
        //    {
        //        return NotFound();
        //    }

        //    await _adminService.DeleteLocationAsync(id);
        //    return NoContent();
        //}
        //// End Location management ================================================================================================

        // Start Post management ================================================================================================
        [HttpGet("Posts")]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _adminService.GetAllPostsAsync();

            if (posts == null || !posts.Any())
            {
                return NotFound();
            }

            var postResponseDtos = posts.Select(post => new PostResponseDto
            {
                PostId = post.PostId,
                AccountId = post.AccountId, // Use AccountId
                UserName = post.Account != null ? post.Account.UserName : "N/A", // Get UserName from Account, if available
                Title = post.Title,
                Content = post.Content,
                Date = post.Date,
                Comments = post.Comments.Select(comment => new CommentResponseDto
                {
                    CommentId = comment.CommentId,
                    AccountId = comment.AccountId, // Include AccountId if needed
                    PostId = comment.PostId,
                    Content = comment.Content,
                    Date = comment.Date,
                    UserName = comment.Account != null ? comment.Account.UserName : "N/A" // Get UserName from Account in Comment, if available
                }).ToList()
            });

            return Ok(postResponseDtos);
        }



        [HttpGet("Posts/{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _adminService.GetPostByIdAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            var postResponseDto = new PostResponseDto
            {
                PostId = post.PostId,
                AccountId = post.AccountId, // Use AccountId
                UserName = post.Account != null ? post.Account.UserName : "N/A", // Get UserName from Account, if available
                Title = post.Title,
                Content = post.Content,
                Date = post.Date,
                Comments = post.Comments.Select(comment => new CommentResponseDto
                {
                    CommentId = comment.CommentId,
                    AccountId = comment.AccountId, // Include AccountId if needed
                    PostId = comment.PostId,
                    Content = comment.Content,
                    Date = comment.Date,
                    UserName = comment.Account != null ? comment.Account.UserName : "N/A" // Get UserName from Account in Comment, if available
                }).ToList()
            };

            return Ok(postResponseDto);
        }

        [HttpPost("posts")]
        public async Task<ActionResult<PostResponseDto>> AddPost([FromBody] PostDto postDto)
        {
            var postResponse = await _adminService.AddPostAsync(postDto);
            return CreatedAtAction(nameof(GetAllPosts), new { postId = postResponse.PostId }, postResponse);
        }

        // UpdatePostById Controller Method
        [HttpPut("Posts/{id}")]
        public async Task<IActionResult> UpdatePostById(int id, [FromBody] PostDto postDto)
        {
            var updatedPost = await _adminService.UpdatePostByIdAsync(id, postDto);

            if (updatedPost == null)
            {
                return NotFound(); // Handle case where post is not found
            }

            return Ok(updatedPost);
        }

        [HttpDelete("Posts/{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _adminService.DeletePostAsync(id);
            return NoContent();
        }
        // End Post management ================================================================================================

        private bool AdminExists(int id)
        {
            return (_context.Admins?.Any(e => e.AdminId == id)).GetValueOrDefault();
        }

        [HttpGet("Comments")]
        public async Task<IActionResult> GetAllComments()
        {
            var comments = await _adminService.GetAllCommentsAsync();
            var commentDtos = comments.Select(c => new CommentResponseDto
            {
                CommentId = c.CommentId,
                AccountId = c.AccountId,
                UserName = c.Account?.UserName ?? "Unknown", // Include UserName from Account
                PostId = c.PostId,
                Content = c.Content,
                Date = c.Date,
            }).ToList();

            return Ok(commentDtos);
        }

        // GET: api/Admins/Comments/5
        [HttpGet("Comments/{id}")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            var comment = await _adminService.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            var commentDto = new CommentResponseDto
            {
                CommentId = comment.CommentId,
                AccountId = comment.AccountId,
                UserName = comment.Account?.UserName ?? "Unknown", // Include UserName from Account
                PostId = comment.PostId,
                Content = comment.Content,
                Date = comment.Date,
            };

            return Ok(commentDto);
        }

        [HttpPost("comments")]
        public async Task<ActionResult<CommentResponseDto>> AddComment([FromBody] CommentDto commentDto)
        {
            // Call the service method to add the comment and get the response DTO
            CommentResponseDto commentResponseDto = await _adminService.AddCommentAsync(commentDto);

            // Return the response DTO with UserName populated
            return Ok(commentResponseDto);
        }

        [HttpPut("Comments/{id}")]
        public async Task<IActionResult> UpdateCommentById(int id, [FromBody] CommentDto commentDto)
        {
            var updatedComment = await _adminService.UpdateCommentByIdAsync(id, commentDto);

            if (updatedComment == null)
            {
                return NotFound(); // Handle case where comment is not found
            }

            return Ok(updatedComment);
        }

        // DELETE: api/Admins/Comments/5
        [HttpDelete("Comments/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _adminService.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            await _adminService.DeleteCommentAsync(id);
            return NoContent();
        }
        // End Comment management ================================================================================================

        // Existing methods...


    }
}

