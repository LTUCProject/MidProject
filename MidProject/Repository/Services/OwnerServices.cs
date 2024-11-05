using MidProject.Models;
using MidProject.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MidProject.Data;
using MidProject.Models.Dto.Response;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Request;
using System;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MidProject.Repository.Services
{
    public class OwnerServices : IOwner
    {
        private readonly MidprojectDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OwnerServices(MidprojectDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetAccountId() =>
       _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Charging Station Management
        public async Task<IEnumerable<ChargingStationResponseDto>> GetAllChargingStationsAsync(string accountId)
        {
            try
            {
                var owner = await _context.Providers
                    .Include(p => p.ChargingStations)
                    .ThenInclude(cs => cs.Chargers)
                    .FirstOrDefaultAsync(p => p.AccountId == accountId);

                if (owner == null)
                {
                    throw new UnauthorizedAccessException("Owner not found");
                }

                var stationDtos = owner.ChargingStations.Select(cs => new ChargingStationResponseDto
                {
                    ChargingStationId = cs.ChargingStationId,
                    StationLocation = cs.StationLocation,
                    Name = cs.Name,
                    HasParking = cs.HasParking,
                    Status = cs.Status,
                    PaymentMethod = cs.PaymentMethod,
                    Address = cs.Address,               // Location properties
                    Latitude = cs.Latitude,
                    Longitude = cs.Longitude,
                    Chargers = cs.Chargers.Select(c => new ChargerResponseDto
                    {
                        ChargerId = c.ChargerId,
                        Type = c.Type,
                        Power = c.Power,
                        Speed = c.Speed,
                        ChargingStationId = c.ChargingStationId
                    }).ToList()
                });

                return stationDtos;
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("An error occurred while retrieving charging stations.", ex);
            }
        }

        public async Task<ChargingStationResponseDto> GetChargingStationByIdAsync(int stationId)
        {
            try
            {
                var station = await _context.ChargingStations
                    .Include(cs => cs.Chargers)
                    .FirstOrDefaultAsync(cs => cs.ChargingStationId == stationId);

                if (station == null) return null;

                return new ChargingStationResponseDto
                {
                    ChargingStationId = station.ChargingStationId,
                    StationLocation = station.StationLocation,
                    Name = station.Name,
                    HasParking = station.HasParking,
                    Status = station.Status,
                    PaymentMethod = station.PaymentMethod,
                    Address = station.Address,           // Location properties
                    Latitude = station.Latitude,
                    Longitude = station.Longitude,
                    Chargers = station.Chargers.Select(c => new ChargerResponseDto
                    {
                        ChargerId = c.ChargerId,
                        Type = c.Type,
                        Power = c.Power,
                        Speed = c.Speed,
                        ChargingStationId = c.ChargingStationId
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("An error occurred while retrieving the charging station by ID.", ex);
            }
        }

        public async Task<ChargingStationResponseDto> CreateChargingStationAsync(ChargingStationDto stationDtoRequest, string accountId)
        {
            try
            {
                var owner = await _context.Providers.FirstOrDefaultAsync(p => p.AccountId == accountId);

                if (owner == null)
                {
                    throw new UnauthorizedAccessException("Owner not found");
                }

                var station = new ChargingStation
                {
                    StationLocation = stationDtoRequest.StationLocation,
                    Name = stationDtoRequest.Name,
                    HasParking = stationDtoRequest.HasParking,
                    Status = stationDtoRequest.Status,
                    PaymentMethod = stationDtoRequest.PaymentMethod,
                    Address = stationDtoRequest.Address,           // Location properties
                    Latitude = stationDtoRequest.Latitude,
                    Longitude = stationDtoRequest.Longitude,
                    ProviderId = owner.ProviderId // Set the owner as the provider
                };

                await _context.ChargingStations.AddAsync(station);
                await _context.SaveChangesAsync();

                return new ChargingStationResponseDto
                {
                    ChargingStationId = station.ChargingStationId,
                    StationLocation = station.StationLocation,
                    Name = station.Name,
                    HasParking = station.HasParking,
                    Status = station.Status,
                    PaymentMethod = station.PaymentMethod,
                    Address = station.Address,                   // Location properties
                    Latitude = station.Latitude,
                    Longitude = station.Longitude
                };
            }
            catch (DbUpdateException ex)
            {
                // Log the error
                throw new Exception("An error occurred while creating the charging station.", ex);
            }
        }

        public async Task UpdateChargingStationAsync(int stationId, ChargingStationDto stationDtoRequest, string accountId)
        {
            try
            {
                var owner = await _context.Providers.FirstOrDefaultAsync(p => p.AccountId == accountId);
                var station = await _context.ChargingStations.FindAsync(stationId);

                if (owner == null || station == null || station.ProviderId != owner.ProviderId)
                {
                    throw new UnauthorizedAccessException("Access denied or station not found");
                }

                station.StationLocation = stationDtoRequest.StationLocation;
                station.Name = stationDtoRequest.Name;
                station.HasParking = stationDtoRequest.HasParking;
                station.Status = stationDtoRequest.Status;
                station.PaymentMethod = stationDtoRequest.PaymentMethod;
                station.Address = stationDtoRequest.Address;           // Location properties
                station.Latitude = stationDtoRequest.Latitude;
                station.Longitude = stationDtoRequest.Longitude;

                _context.ChargingStations.Update(station);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the error
                throw new Exception("An error occurred while updating the charging station.", ex);
            }
        }


        public async Task DeleteChargingStationAsync(int stationId, string accountId)
        {
            try
            {
                var owner = await _context.Providers.FirstOrDefaultAsync(p => p.AccountId == accountId);
                var station = await _context.ChargingStations.FindAsync(stationId);

                if (owner == null || station == null || station.ProviderId != owner.ProviderId)
                {
                    throw new UnauthorizedAccessException("Access denied or station not found");
                }

                _context.ChargingStations.Remove(station);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the error
                throw new Exception("An error occurred while deleting the charging station.", ex);
            }
        }


        // Charger Management
        public async Task<IEnumerable<ChargerResponseDto>> GetChargersAsync(int stationId)
        {
            try
            {
                var chargers = await _context.Chargers
                    .Where(c => c.ChargingStationId == stationId)
                    .ToListAsync();

                return chargers.Select(c => new ChargerResponseDto
                {
                    ChargerId = c.ChargerId,
                    Type = c.Type,
                    Power = c.Power,
                    Speed = c.Speed,
                    ChargingStationId = c.ChargingStationId
                });
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("An error occurred while retrieving chargers.", ex);
            }
        }

        public async Task<ChargerResponseDto> GetChargerByIdAsync(int chargerId)
        {
            try
            {
                var charger = await _context.Chargers
                    .FirstOrDefaultAsync(c => c.ChargerId == chargerId);

                if (charger == null) return null;

                return new ChargerResponseDto
                {
                    ChargerId = charger.ChargerId,
                    Type = charger.Type,
                    Power = charger.Power,
                    Speed = charger.Speed,
                    ChargingStationId = charger.ChargingStationId
                };
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("An error occurred while retrieving the charger by ID.", ex);
            }
        }

        public async Task<Charger> CreateChargerAsync(ChargerDto chargerDtoRequest)
        {

            var charger = new Charger
            {
                Type = chargerDtoRequest.Type,
                Power = chargerDtoRequest.Power,
                Speed = chargerDtoRequest.Speed,
                ChargingStationId = chargerDtoRequest.ChargingStationId
            };

            await _context.Chargers.AddAsync(charger);
            await _context.SaveChangesAsync();
            return charger;

        }

        public async Task UpdateChargerAsync(int chargerId, ChargerDto chargerDtoRequest)
        {
            try
            {
                var charger = await _context.Chargers.FindAsync(chargerId);

                if (charger == null) return;

                charger.Type = chargerDtoRequest.Type;
                charger.Power = chargerDtoRequest.Power;
                charger.Speed = chargerDtoRequest.Speed;
                charger.ChargingStationId = chargerDtoRequest.ChargingStationId;

                _context.Chargers.Update(charger);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the error
                throw new Exception("An error occurred while updating the charger.", ex);
            }
        }

        public async Task DeleteChargerAsync(int chargerId)
        {
            try
            {
                var charger = await _context.Chargers.FindAsync(chargerId);

                if (charger != null)
                {
                    _context.Chargers.Remove(charger);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                // Log the error
                throw new Exception("An error occurred while deleting the charger.", ex);
            }
        }

        // Maintenance Logs Management
        public async Task<IEnumerable<MaintenanceLogDtoResponse>> GetMaintenanceLogsAsync(int stationId)
        {
            try
            {
                var logs = await _context.MaintenanceLogs
                    .Where(m => m.ChargingStationId == stationId)
                    .ToListAsync();

                return logs.Select(m => new MaintenanceLogDtoResponse
                {
                    MaintenanceLogId = m.MaintenanceLogId,
                    ChargingStationId = m.ChargingStationId,
                    MaintenanceDate = m.MaintenanceDate,
                    PerformedBy = m.PerformedBy,
                    Details = m.Details,
                    Cost = m.Cost
                });
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("An error occurred while retrieving maintenance logs.", ex);
            }
        }

        public async Task<MaintenanceLog> AddMaintenanceLogAsync(MaintenanceLogDto logDtoRequest)
        {

            var log = new MaintenanceLog
            {
                ChargingStationId = logDtoRequest.ChargingStationId,
                MaintenanceDate = logDtoRequest.MaintenanceDate,
                PerformedBy = logDtoRequest.PerformedBy,
                Details = logDtoRequest.Details,
                Cost = logDtoRequest.Cost
            };

            await _context.MaintenanceLogs.AddAsync(log);
            await _context.SaveChangesAsync();
            return log;

        }

        public async Task RemoveMaintenanceLogAsync(int logId)
        {
            try
            {
                var log = await _context.MaintenanceLogs.FindAsync(logId);

                if (log != null)
                {
                    _context.MaintenanceLogs.Remove(log);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                // Log the error
                throw new Exception("An error occurred while deleting the maintenance log.", ex);
            }
        }


        //Notification

        public async Task<NotificationResponseDto> CreateNotificationAsync(NotificationDto notificationDto)
        {
            var notification = new Notification
            {
                ClientId = notificationDto.ClientId, // Ensure this is correct and matches the Client entity type
                Title = notificationDto.Title,
                Message = notificationDto.Message,
                Date = notificationDto.Date
            };

            await _context.Notifications.AddAsync(notification); // Add the notification to the context
            await _context.SaveChangesAsync(); // Save changes to the database

            // Return the created notification's details
            return new NotificationResponseDto
            {
                NotificationId = notification.NotificationId, // ID generated by the database
                ClientId = notification.ClientId,
                Title = notification.Title,
                Message = notification.Message,
                Date = notification.Date
            };
        }


        public async Task<NotificationResponseDto> GetNotificationByIdAsync(int notificationId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.NotificationId == notificationId);

            if (notification == null)
            {
                return null;
            }

            return new NotificationResponseDto
            {
                NotificationId = notification.NotificationId,
                ClientId = notification.ClientId,
                Title = notification.Title,
                Message = notification.Message,
                Date = notification.Date
            };
        }

        public async Task<IEnumerable<NotificationResponseDto>> GetNotificationsByClientIdAsync(int clientId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.ClientId == clientId)
                .ToListAsync();

            return notifications.Select(n => new NotificationResponseDto
            {
                NotificationId = n.NotificationId,
                ClientId = n.ClientId,
                Title = n.Title,
                Message = n.Message,
                Date = n.Date
            });
        }
        ////Location
        //public async Task<IEnumerable<LocationResponseDto>> GetAllLocationsAsync()
        //{
        //    return await _context.Locations
        //        .Select(loc => new LocationResponseDto
        //        {
        //            LocationId = loc.LocationId,
        //            Name = loc.Name,
        //            Address = loc.Address,
        //            Latitude = loc.Latitude,
        //            Longitude = loc.Longitude,
        //            ChargingStations = loc.ChargingStations.Select(cs => new ChargingStationResponseDto
        //            {
        //                ChargingStationId = cs.ChargingStationId,
        //                StationLocation = cs.StationLocation,
        //                LocationId = cs.LocationId,
        //                Name = cs.Name,
        //                HasParking = cs.HasParking,
        //                Status = cs.Status,
        //                PaymentMethod = cs.PaymentMethod
        //            })
        //        }).ToListAsync();
        //}

        //public async Task<LocationResponseDto> GetLocationByIdAsync(int id)
        //{
        //    return await _context.Locations
        //        .Where(loc => loc.LocationId == id)
        //        .Select(loc => new LocationResponseDto
        //        {
        //            LocationId = loc.LocationId,
        //            Name = loc.Name,
        //            Address = loc.Address,
        //            Latitude = loc.Latitude,
        //            Longitude = loc.Longitude,
        //            ChargingStations = loc.ChargingStations.Select(cs => new ChargingStationResponseDto
        //            {
        //                // Map ChargingStation properties here
        //            })
        //        }).FirstOrDefaultAsync();
        //}

        //public async Task<LocationResponseDto> CreateLocationAsync(LocationDto locationDto)
        //{
        //    var location = new Location
        //    {
        //        Name = locationDto.Name,
        //        Address = locationDto.Address,
        //        Latitude = locationDto.Latitude,
        //        Longitude = locationDto.Longitude
        //    };

        //    _context.Locations.Add(location);
        //    await _context.SaveChangesAsync();

        //    return new LocationResponseDto
        //    {
        //        LocationId = location.LocationId,
        //        Name = location.Name,
        //        Address = location.Address,
        //        Latitude = location.Latitude,
        //        Longitude = location.Longitude
        //    };
        //}

        //public async Task UpdateLocationAsync(int id, LocationDto locationDto)
        //{
        //    var location = await _context.Locations.FindAsync(id);
        //    if (location == null) throw new KeyNotFoundException("Location not found.");

        //    location.Name = locationDto.Name;
        //    location.Address = locationDto.Address;
        //    location.Latitude = locationDto.Latitude;
        //    location.Longitude = locationDto.Longitude;

        //    _context.Locations.Update(location);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task DeleteLocationAsync(int id)
        //{
        //    var location = await _context.Locations.FindAsync(id);
        //    if (location == null) throw new KeyNotFoundException("Location not found.");

        //    _context.Locations.Remove(location);
        //    await _context.SaveChangesAsync();
        //}

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _context.Posts
                .Include(p => p.Account)  // Include Account to fetch UserName
                .Include(p => p.Comments) // Include Comments if needed
                .ToListAsync();
        }


        public async Task<PostResponseDto> AddPostAsync(PostDto postDto)
        {
            // Create a new Post instance from the provided PostDto
            var post = new Post
            {
                AccountId = postDto.AccountId, // Set AccountId instead of ClientId
                Title = postDto.Title,
                Content = postDto.Content,
                Date = postDto.Date
            };

            // Add the post to the context and save changes
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            // Fetch the user information to populate AccountId and UserName in PostResponseDto
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == post.AccountId); // Fetch the user by Id

            // Return the PostResponseDto
            return new PostResponseDto
            {
                PostId = post.PostId,
                AccountId = post.AccountId, // Use AccountId
                UserName = user != null ? user.UserName : "Unknown", // Use UserName from IdentityUser
                Title = post.Title,
                Content = post.Content,
                Date = post.Date,
                Comments = new List<CommentResponseDto>() // Initialize empty list
            };
        }


        public async Task<PostResponseDto> UpdatePostByIdAsync(int postId, PostDto postDto)
        {
            var post = await _context.Posts
                .Include(p => p.Comments) // Include Comments to avoid null references
                .FirstOrDefaultAsync(p => p.PostId == postId);

            if (post == null)
            {
                return null; // Handle post not found
            }

            // Update post fields
            post.Title = postDto.Title;
            post.Content = postDto.Content;
            post.Date = postDto.Date;

            await _context.SaveChangesAsync();

            // Fetch updated UserName from Account
            var userName = await _context.Users
                .Where(u => u.Id == post.AccountId)
                .Select(u => u.UserName)
                .FirstOrDefaultAsync();

            return new PostResponseDto
            {
                PostId = post.PostId,
                AccountId = post.AccountId,
                UserName = userName ?? "Unknown",
                Title = post.Title,
                Content = post.Content,
                Date = post.Date,
                Comments = post.Comments?.Select(comment => new CommentResponseDto
                {
                    CommentId = comment.CommentId,
                    AccountId = comment.AccountId,
                    PostId = comment.PostId,
                    Content = comment.Content,
                    Date = comment.Date,
                    UserName = comment.Account != null ? comment.Account.UserName : "Unknown"
                }).ToList() ?? new List<CommentResponseDto>() // Handle null Comments collection
            };
        }


        public async Task DeletePostAsync(int postId)
        {
            var post = await _context.Posts
                .Include(p => p.Comments) // Include comments to handle related data
                .FirstOrDefaultAsync(p => p.PostId == postId);

            if (post != null)
            {
                _context.Comments.RemoveRange(post.Comments); // Remove related comments
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CommentResponseDto>> GetAllCommentsAsync()
        {
            return await _context.Comments
                .Select(c => new CommentResponseDto
                {
                    CommentId = c.CommentId,
                    AccountId = c.AccountId,
                    PostId = c.PostId,
                    Content = c.Content,
                    Date = c.Date,
                    UserName = _context.Users
                        .Where(u => u.Id == c.AccountId)
                        .Select(u => u.UserName)
                        .FirstOrDefault()
                })
                .ToListAsync();
        }


        public async Task<CommentResponseDto> AddCommentAsync(CommentDto commentDto)
        {
            // Create and save the comment
            var comment = new Comment
            {
                AccountId = commentDto.AccountId,
                PostId = commentDto.PostId,
                Content = commentDto.Content,
                Date = commentDto.Date
            };

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            // Fetch the UserName from the AccountId
            var userName = await _context.Users
                .Where(u => u.Id == comment.AccountId)
                .Select(u => u.UserName) // Fetch UserName directly
                .FirstOrDefaultAsync();

            // Return the CommentResponseDto with UserName
            return new CommentResponseDto
            {
                CommentId = comment.CommentId,
                AccountId = comment.AccountId,
                PostId = comment.PostId,
                Content = comment.Content,
                Date = comment.Date,
                UserName = userName ?? "Unknown" // Use UserName or "Unknown" if null
            };
        }


        public async Task<CommentResponseDto> UpdateCommentByIdAsync(int commentId, CommentDto commentDto)
        {
            var comment = await _context.Comments.FindAsync(commentId);

            if (comment == null)
            {
                return null; // Handle comment not found
            }

            // Update comment fields
            comment.Content = commentDto.Content;
            comment.Date = commentDto.Date;

            await _context.SaveChangesAsync();

            // Fetch updated UserName from Account
            var userName = await _context.Users
                .Where(u => u.Id == comment.AccountId)
                .Select(u => u.UserName)
                .FirstOrDefaultAsync();

            return new CommentResponseDto
            {
                CommentId = comment.CommentId,
                AccountId = comment.AccountId,
                PostId = comment.PostId,
                Content = comment.Content,
                Date = comment.Date,
                UserName = userName ?? "Unknown"
            };
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }



        // Booking management
        public async Task<IEnumerable<BookingDto>> GetBookingsByChargingStationAsync(int stationId)
        {
            var accountId = GetAccountId();
            var owner = await _context.Providers
                .Include(p => p.ChargingStations)
                .FirstOrDefaultAsync(p => p.AccountId == accountId);

            if (owner == null || !owner.ChargingStations.Any(cs => cs.ChargingStationId == stationId))
            {
                throw new UnauthorizedAccessException("You are not authorized to access these bookings.");
            }

            return await _context.Bookings
                .Include(b => b.Client) // Include Client data
                .Include(b => b.Vehicle) // Include Vehicle data if necessary
                .Where(b => b.ChargingStationId == stationId)
                .Select(b => new BookingDto
                {
                    BookingId = b.BookingId,
                    ClientId = b.ClientId,
                    ClientName = b.Client.Name,
                    ClientEmail = b.Client.Email,
                    VehicleModel = b.Vehicle.Model, // Changed to VehicleModel assuming Vehicle is a navigation property
                    ChargingStationId = b.ChargingStationId,
                    VehicleId = b.VehicleId,
                    StartTime = b.StartTime,
                    EndTime = b.EndTime,
                    Status = b.Status,
                    Cost = b.Cost
                })
                .ToListAsync();
        }



        public async Task<BookingDto> GetBookingByIdAsync(int bookingId)
        {
            var accountId = GetAccountId();
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null)
                return null;

            var owner = await _context.Providers
                .Include(p => p.ChargingStations)
                .FirstOrDefaultAsync(p => p.AccountId == accountId);

            if (owner == null || !owner.ChargingStations.Any(cs => cs.ChargingStationId == booking.ChargingStationId))
            {
                throw new UnauthorizedAccessException("You are not authorized to access this booking.");
            }

            return new BookingDto
            {
                BookingId = booking.BookingId,
                ClientId = booking.ClientId,
                ChargingStationId = booking.ChargingStationId,
                VehicleId = booking.VehicleId,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                Status = booking.Status,
                Cost = booking.Cost
            };
        }


        public async Task<IEnumerable<BookingDto>> GetBookingsByDateRangeAsync(int stationId, DateTime startDate, DateTime endDate)
        {
            var accountId = GetAccountId();
            var owner = await _context.Providers
                .Include(p => p.ChargingStations)
                .FirstOrDefaultAsync(p => p.AccountId == accountId);

            if (owner == null || !owner.ChargingStations.Any(cs => cs.ChargingStationId == stationId))
            {
                throw new UnauthorizedAccessException("You are not authorized to access these bookings.");
            }

            return await _context.Bookings
                .Where(b => b.ChargingStationId == stationId && b.StartTime >= startDate && b.EndTime <= endDate)
                .Select(b => new BookingDto
                {
                    BookingId = b.BookingId,
                    ClientId = b.ClientId,
                    ChargingStationId = b.ChargingStationId,
                    VehicleId = b.VehicleId,
                    StartTime = b.StartTime,
                    EndTime = b.EndTime,
                    Status = b.Status,
                    Cost = b.Cost
                })
                .ToListAsync();
        }


        public async Task<IEnumerable<BookingDto>> GetPendingBookingsByChargingStationAsync(int stationId)
        {
            var accountId = GetAccountId();
            var owner = await _context.Providers
                .Include(p => p.ChargingStations)
                .FirstOrDefaultAsync(p => p.AccountId == accountId);

            if (owner == null || !owner.ChargingStations.Any(cs => cs.ChargingStationId == stationId))
            {
                throw new UnauthorizedAccessException("You are not authorized to access these bookings.");
            }

            return await _context.Bookings
                .Include(b => b.Client) // Include Client data
                .Include(b => b.Vehicle) // Include Vehicle data if necessary
                .Where(b => b.ChargingStationId == stationId && b.Status == "Pending")
                .Select(b => new BookingDto
                {
                    BookingId = b.BookingId,
                    ClientId = b.ClientId,
                    ClientName = b.Client.Name,
                    ClientEmail = b.Client.Email,
                    VehicleModel = b.Vehicle.Model, // Changed to VehicleModel assuming Vehicle is a navigation property
                    ChargingStationId = b.ChargingStationId,
                    VehicleId = b.VehicleId,
                    StartTime = b.StartTime,
                    EndTime = b.EndTime,
                    Status = b.Status,
                    Cost = b.Cost
                })
                .ToListAsync();
        }

        public async Task UpdateBookingDetailsAsync(int bookingId, string newStatus, int newCost)
        {
            var accountId = GetAccountId();
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null)
                throw new KeyNotFoundException("Booking not found.");

            var owner = await _context.Providers
                .Include(p => p.ChargingStations)
                .FirstOrDefaultAsync(p => p.AccountId == accountId);

            if (owner == null || !owner.ChargingStations.Any(cs => cs.ChargingStationId == booking.ChargingStationId))
            {
                throw new UnauthorizedAccessException("You are not authorized to update this booking.");
            }

            booking.Status = newStatus;
            booking.Cost = newCost;

            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }


        //Session
        // Session management
        public async Task<IEnumerable<SessionDtoResponse>> GetSessionsByChargingStationAsync(int stationId)
        {
            var accountId = GetAccountId(); // Get the account ID from the context
            var owner = await _context.Providers
                .Include(p => p.ChargingStations)
                .FirstOrDefaultAsync(p => p.AccountId == accountId);

            if (owner == null)
            {
                throw new UnauthorizedAccessException("Owner not found");
            }

            return await _context.Sessions
                .Where(s => s.ChargingStationId == stationId && owner.ChargingStations.Any(cs => cs.ChargingStationId == stationId))
                .Select(s => new SessionDtoResponse
                {
                    SessionId = s.SessionId,
                    ClientId = s.ClientId,
                    ChargingStationId = s.ChargingStationId,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    EnergyConsumed = s.EnergyConsumed,
                    Cost = s.Cost
                })
                .ToListAsync();
        }

        public async Task<SessionDtoResponse> GetSessionByIdAsync(int sessionId)
        {
            var accountId = GetAccountId(); // Get the account ID from the context
            var owner = await _context.Providers
                .Include(p => p.ChargingStations)
                .FirstOrDefaultAsync(p => p.AccountId == accountId);

            if (owner == null)
            {
                throw new UnauthorizedAccessException("Owner not found");
            }

            var session = await _context.Sessions
                .Where(s => s.SessionId == sessionId && owner.ChargingStations.Any(cs => cs.ChargingStationId == s.ChargingStationId))
                .Select(s => new SessionDtoResponse
                {
                    SessionId = s.SessionId,
                    ClientId = s.ClientId,
                    ChargingStationId = s.ChargingStationId,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    EnergyConsumed = s.EnergyConsumed,
                    Cost = s.Cost
                })
                .FirstOrDefaultAsync();

            if (session == null)
            {
                throw new UnauthorizedAccessException("Session not found for this owner");
            }

            return session;
        }

        public async Task UpdateSessionDetailsAsync(int sessionId, int energyConsumed, int cost)
        {
            var accountId = GetAccountId(); // Get the account ID from the context
            var owner = await _context.Providers
                .Include(p => p.ChargingStations)
                .FirstOrDefaultAsync(p => p.AccountId == accountId);

            if (owner == null)
            {
                throw new UnauthorizedAccessException("Owner not found");
            }

            var session = await _context.Sessions
                .FirstOrDefaultAsync(s => s.SessionId == sessionId && owner.ChargingStations.Any(cs => cs.ChargingStationId == s.ChargingStationId));

            if (session != null)
            {
                session.EnergyConsumed = energyConsumed;
                session.Cost = cost;

                _context.Sessions.Update(session);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new UnauthorizedAccessException("Session not found for this owner");
            }
        }

    }
}