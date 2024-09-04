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
                AccountId = p.AccountId,
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
                AccountId = provider.AccountId,
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
        // End Subscription plan management ================================================================================================

        // Start Notifications management ================================================================================================
        // GET: api/Admin/Clients/{clientId}/Notifications
        [HttpGet("Clients/{clientId}/Notifications")]
        public async Task<IActionResult> GetClientNotifications(int clientId)
        {
            var notifications = await _adminService.GetClientNotificationsAsync(clientId);
            if (notifications == null)
            {
                return NotFound();
            }
            return Ok(notifications);
        }

        // POST: api/Admin/Clients/{clientId}/Notifications
        [HttpPost("Clients/{clientId}/Notifications")]
        public async Task<IActionResult> AddNotification(int clientId, [FromBody] NotificationRequestDto notificationDto)
        {
            if (clientId != notificationDto.ClientId)
            {
                return BadRequest("Client ID in the URL does not match Client ID in the request body.");
            }

            await _adminService.AddNotificationAsync(notificationDto);
            return CreatedAtAction(nameof(GetClientNotifications), new { clientId = notificationDto.ClientId }, notificationDto);
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

        // Start Booking management ================================================================================================
        [HttpGet("Bookings")]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _adminService.GetAllBookingsAsync();
            var bookingDtos = bookings.Select(b => new BookingResponseDto
            {
                BookingId = b.BookingId,
                ClientId = b.ClientId,
                ServiceInfoId = b.ServiceInfoId,
                VehicleId = b.VehicleId,
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                Status = b.Status,
                Cost = b.Cost
            });
            return Ok(bookingDtos);
        }

        [HttpGet("Bookings/{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking = await _adminService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            var bookingDto = new BookingResponseDto
            {
                BookingId = booking.BookingId,
                ClientId = booking.ClientId,
                ServiceInfoId = booking.ServiceInfoId,
                VehicleId = booking.VehicleId,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                Status = booking.Status,
                Cost = booking.Cost
            };
            return Ok(bookingDto);
        }

        [HttpDelete("Bookings/{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _adminService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            await _adminService.DeleteBookingAsync(id);
            return NoContent();
        }
        // End Booking management ================================================================================================

        // Start Location management ================================================================================================
        [HttpGet("Locations")]
        public async Task<IActionResult> GetAllLocations()
        {
            var locations = await _adminService.GetAllLocationsAsync();
            var locationDtos = locations.Select(l => new LocationResponseDto
            {
                LocationId = l.LocationId,
                Name = l.Name,
                Address = l.Address,
                Latitude = l.Latitude,
                Longitude = l.Longitude,
                ChargingStations = l.ChargingStations.Select(cs => new ChargingStationResponseDto
                {
                    ChargingStationId = cs.ChargingStationId,
                    StationLocation = cs.StationLocation,
                    Name = cs.Name,
                    HasParking = cs.HasParking,
                    Status = cs.Status,
                    PaymentMethod = cs.PaymentMethod
                })
            });
            return Ok(locationDtos);
        }

        [HttpGet("Locations/{id}")]
        public async Task<IActionResult> GetLocationById(int id)
        {
            var location = await _adminService.GetLocationByIdAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            var locationDto = new LocationResponseDto
            {
                LocationId = location.LocationId,
                Name = location.Name,
                Address = location.Address,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                ChargingStations = location.ChargingStations.Select(cs => new ChargingStationResponseDto
                {
                    ChargingStationId = cs.ChargingStationId,
                    StationLocation = cs.StationLocation,
                    Name = cs.Name,
                    HasParking = cs.HasParking,
                    Status = cs.Status,
                    PaymentMethod = cs.PaymentMethod
                })
            };
            return Ok(locationDto);
        }

        [HttpPost("Locations")]
        public async Task<IActionResult> CreateLocation([FromBody] LocationRequestDto locationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var location = new Location
            {
                Name = locationDto.Name,
                Address = locationDto.Address,
                Latitude = locationDto.Latitude,
                Longitude = locationDto.Longitude
            };

            await _adminService.CreateLocationAsync(location);
            return CreatedAtAction(nameof(GetLocationById), new { id = location.LocationId }, location);
        }

        [HttpPut("Locations/{id}")]
        public async Task<IActionResult> UpdateLocation(int id, [FromBody] LocationRequestDto locationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var location = await _adminService.GetLocationByIdAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            location.Name = locationDto.Name;
            location.Address = locationDto.Address;
            location.Latitude = locationDto.Latitude;
            location.Longitude = locationDto.Longitude;

            await _adminService.UpdateLocationAsync(location);
            return NoContent();
        }

        [HttpDelete("Locations/{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var location = await _adminService.GetLocationByIdAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            await _adminService.DeleteLocationAsync(id);
            return NoContent();
        }
        // End Location management ================================================================================================

        // Start Post management ================================================================================================
        [HttpGet("Posts")]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _adminService.GetAllPostsAsync();

            if (posts == null)
            {
                return NotFound();
            }

            var postResponseDtos = posts.Select(post => new PostResponseDto
            {
                PostId = post.PostId,
                ClientId = post.Client.ClientId,
                ClientName = post.Client.Name,
                Title = post.Title,
                Content = post.Content,
                Date = post.Date,
                Comments = post.Comments.Select(comment => new CommentResponseDto
                {
                    CommentId = comment.CommentId,
                    PostId = comment.PostId,
                    Content = comment.Content,
                    Date = comment.Date
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
                ClientId = post.Client.ClientId,
                ClientName = post.Client.Name,
                Title = post.Title,
                Content = post.Content,
                Date = post.Date,
                Comments = post.Comments.Select(comment => new CommentResponseDto
                {
                    CommentId = comment.CommentId,
                    PostId = comment.PostId,
                    Content = comment.Content,
                    Date = comment.Date
                }).ToList()
            };

            return Ok(postResponseDto);
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
    }
}
