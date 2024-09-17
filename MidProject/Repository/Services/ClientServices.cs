using System.Linq;
using Microsoft.EntityFrameworkCore;
using MidProject.Data;
using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;
using MidProject.Repository.Interfaces;

namespace MidProject.Repository.Services
{
    public class ClientServices : IClient
    {
        private readonly MidprojectDbContext _context;

        public ClientServices(MidprojectDbContext context)
        {
            _context = context;
        }

        // Session management
        public async Task<IEnumerable<Session>> GetClientSessionsAsync(int clientId)
        {
            return await _context.Sessions
                .Where(s => s.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<Session> GetSessionByIdAsync(int sessionId)
        {
            return await _context.Sessions
                .FirstOrDefaultAsync(s => s.SessionId == sessionId);
        }

        public async Task<Session> StartSessionAsync(SessionDto sessionDto)
        {
            var session = new Session
            {
                ClientId = sessionDto.ClientId,
                ChargingStationId = sessionDto.ChargingStationId,
                StartTime = sessionDto.StartTime,
                EndTime = sessionDto.EndTime,
                EnergyConsumed = sessionDto.EnergyConsumed
            };

            await _context.Sessions.AddAsync(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task EndSessionAsync(int sessionId)
        {
            var session = await _context.Sessions.FindAsync(sessionId);
            if (session != null)
            {
                session.EndTime = DateTime.UtcNow;
                _context.Sessions.Update(session);
                await _context.SaveChangesAsync();
            }
        }

        // Payment transactions management
        public async Task<IEnumerable<PaymentTransaction>> GetClientPaymentsAsync(int sessionId)
        {
            return await _context.PaymentTransactions
                .Where(pt => pt.SessionId == sessionId)
                .ToListAsync();
        }

        public async Task<PaymentTransaction> AddPaymentAsync(PaymentTransactionDto paymentDto)
        {
            var payment = new PaymentTransaction
            {
                SessionId = paymentDto.SessionId,
                ClientId = paymentDto.ClientId,
                Amount = paymentDto.Amount,
                PaymentDate = paymentDto.PaymentDate,
                PaymentMethod = paymentDto.PaymentMethod,
                Status = paymentDto.Status
            };

            await _context.PaymentTransactions.AddAsync(payment);
            await _context.SaveChangesAsync();

            return payment;
        }

        public async Task RemovePaymentAsync(int paymentId)
        {
            var payment = await _context.PaymentTransactions.FindAsync(paymentId);
            if (payment != null)
            {
                _context.PaymentTransactions.Remove(payment);
                await _context.SaveChangesAsync();
            }
        }

        // Favorites management
        // Retrieve all charging station favorites for a client with detailed information
        public async Task<IEnumerable<FavoriteChargingStationResponseDto>> GetClientChargingStationFavoritesAsync(int clientId)
        {
            return await _context.ChargingStationFavorites
                .Where(f => f.ClientId == clientId)
                .Include(f => f.ChargingStation) // Include the related ChargingStation entity
                .Select(f => new FavoriteChargingStationResponseDto
                {
                    FavoriteId = f.ChargingStationFavoriteId, // Assuming the primary key is Id
                    ChargingStationId = f.ChargingStationId,
                    ClientId = f.ClientId,
                    ChargingStation = new List<ChargingStation> // Creating a collection with the relevant ChargingStation
                    {
                        f.ChargingStation
                    }
                })
                .ToListAsync();
        }

        // Retrieve all service info favorites for a client with detailed information
        public async Task<IEnumerable<FavoriteServiceInfoResponseDto>> GetClientServiceInfoFavoritesAsync(int clientId)
        {
            return await _context.ServiceInfoFavorites
                .Where(f => f.ClientId == clientId)
                .Include(f => f.ServiceInfo) // Include the related ServiceInfo entity
                .Select(f => new FavoriteServiceInfoResponseDto
                {
                    FavoriteId = f.ServiceInfoFavoriteId, // Assuming the primary key is Id
                    ServiceInfoId = f.ServiceInfoId,
                    ClientId = f.ClientId,
                    ServiceInfo = new List<ServiceInfo> // Creating a collection with the relevant ServiceInfo
                    {
                        f.ServiceInfo
                    }
                })
                .ToListAsync();
        }

        // Add a new charging station favorite
        public async Task<FavoriteChargingStationResponseDto> AddChargingStationFavoriteAsync(FavoriteChargingStationDto favoriteDto)
        {
            var favorite = new ChargingStationFavorite
            {
                ClientId = favoriteDto.ClientId,
                ChargingStationId = favoriteDto.ChargingStationId
            };

            _context.ChargingStationFavorites.Add(favorite);
            await _context.SaveChangesAsync();

            var chargingStation = await _context.ChargingStations.FindAsync(favoriteDto.ChargingStationId);

            return new FavoriteChargingStationResponseDto
            {
                FavoriteId = favorite.ChargingStationFavoriteId, // Assuming the primary key is Id
                ChargingStationId = favorite.ChargingStationId,
                ClientId = favorite.ClientId,
                ChargingStation = new List<ChargingStation> // Creating a collection with the relevant ChargingStation
                {
                    chargingStation
                }
            };
        }

        // Add a new service info favorite
        public async Task<FavoriteServiceInfoResponseDto> AddServiceInfoFavoriteAsync(FavoriteServiceInfoDto favoriteDto)
        {
            var favorite = new ServiceInfoFavorite
            {
                ClientId = favoriteDto.ClientId,
                ServiceInfoId = favoriteDto.ServiceInfoId
            };

            _context.ServiceInfoFavorites.Add(favorite);
            await _context.SaveChangesAsync();

            var serviceInfo = await _context.ServiceInfos.FindAsync(favoriteDto.ServiceInfoId);

            return new FavoriteServiceInfoResponseDto
            {
                FavoriteId = favorite.ServiceInfoFavoriteId, // Assuming the primary key is Id
                ServiceInfoId = favorite.ServiceInfoId,
                ClientId = favorite.ClientId,
                ServiceInfo = new List<ServiceInfo> // Creating a collection with the relevant ServiceInfo
                {
                    serviceInfo
                }
            };
        }

        // Remove a charging station favorite
        public async Task RemoveChargingStationFavoriteAsync(int favoriteId)
        {
            var favorite = await _context.ChargingStationFavorites.FindAsync(favoriteId);
            if (favorite != null)
            {
                _context.ChargingStationFavorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }

        // Remove a service info favorite
        public async Task RemoveServiceInfoFavoriteAsync(int favoriteId)
        {
            var favorite = await _context.ServiceInfoFavorites.FindAsync(favoriteId);
            if (favorite != null)
            {
                _context.ServiceInfoFavorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }

        // Vehicle management
        public async Task<IEnumerable<Vehicle>> GetClientVehiclesAsync(int clientId)
        {
            return await _context.Vehicles
                .Where(v => v.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<Vehicle> GetVehicleByIdAsync(int vehicleId)
        {
            return await _context.Vehicles
                .FirstOrDefaultAsync(v => v.VehicleId == vehicleId);
        }

        public async Task<Vehicle> AddVehicleAsync(VehicleDto vehicleDto)
        {
            var vehicle = new Vehicle
            {
                LicensePlate = vehicleDto.LicensePlate,
                Model = vehicleDto.Model,
                Year = vehicleDto.Year,
                BatteryCapacity = vehicleDto.BatteryCapacity,
                ElectricType = vehicleDto.ElectricType,
                ClientId = vehicleDto.ClientId

            };

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return vehicle;
        }

        public async Task RemoveVehicleAsync(int vehicleId)
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
        }

        //Booking
        public async Task<IEnumerable<Booking>> GetClientBookingsAsync(int clientId)
        {
            return await _context.Bookings
                .Where(b => b.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<Booking> GetBookingByIdAsync(int bookingId)
        {
            return await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);
        }

        public async Task<Booking> AddBookingAsync(BookingDto bookingDto)
        {
            var booking = new Booking
            {
                ClientId = bookingDto.ClientId,
                ServiceInfoId = bookingDto.ServiceInfoId,
                VehicleId = bookingDto.VehicleId,
                StartTime = bookingDto.StartTime,
                EndTime = bookingDto.EndTime,
                Status = bookingDto.Status,
                Cost = bookingDto.Cost
            };

            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();

            return booking;
        }

        public async Task RemoveBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
        }

        // Service request management
        public async Task<IEnumerable<ServiceRequest>> GetClientServiceRequestsAsync(int clientId)
        {
            return await _context.ServiceRequests
                .Where(sr => sr.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<ServiceRequest> GetServiceRequestByIdAsync(int requestId)
        {
            return await _context.ServiceRequests
                .FirstOrDefaultAsync(sr => sr.ServiceRequestId == requestId);
        }

        public async Task<ServiceRequest> CreateServiceRequestAsync(ClientServiceRequestDto requestDto)
        {
            var serviceRequest = new ServiceRequest
            {
                ServiceInfoId = requestDto.ServiceInfoId,
                ClientId = requestDto.ClientId,
                ProviderId = requestDto.ProviderId,
                VehicleId = requestDto.VehicleId, // Add vehicle information to the request
                Status = requestDto.Status
            };

            await _context.ServiceRequests.AddAsync(serviceRequest);
            await _context.SaveChangesAsync();
            return serviceRequest;
        }

        public async Task DeleteServiceRequestAsync(int requestId)
        {
            var serviceRequest = await _context.ServiceRequests.FindAsync(requestId);
            if (serviceRequest != null)
            {
                _context.ServiceRequests.Remove(serviceRequest);
                await _context.SaveChangesAsync();
            }
        }


        // Feedback management
        public async Task<IEnumerable<Feedback>> GetClientFeedbacksAsync(int clientId)
        {
            return await _context.Feedbacks
                .Where(f => f.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<Feedback> AddFeedbackAsync(FeedbackDto feedbackDto)
        {
            var feedback = new Feedback
            {
                ClientId = feedbackDto.ClientId,
                ServiceInfoId = feedbackDto.ServiceInfoId,
                Rating = feedbackDto.Rating,
                Comments = feedbackDto.Comments,
                Date = feedbackDto.Date
            };

            await _context.Feedbacks.AddAsync(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }

        public async Task RemoveFeedbackAsync(int feedbackId)
        {
            var feedback = await _context.Feedbacks.FindAsync(feedbackId);
            if (feedback != null)
            {
                _context.Feedbacks.Remove(feedback);
                await _context.SaveChangesAsync();
            }
        }

        // Notification management
        public async Task<IEnumerable<Notification>> GetClientNotificationsAsync(int clientId)
        {
            return await _context.Notifications
                .Where(n => n.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<Notification> GetNotificationByIdAsync(int notificationId)
        {
            return await _context.Notifications
                .FirstOrDefaultAsync(n => n.NotificationId == notificationId);
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _context.Posts
                .Include(p => p.Comments) // Include comments if needed
                .ToListAsync();
        }

        public async Task<Post> AddPostAsync(PostDto postDto)
        {
            var post = new Post
            {
                ClientId = postDto.ClientId,
                Title = postDto.Title,
                Content = postDto.Content,
                Date = postDto.Date
            };

            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            return post;
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

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            return await _context.Comments
                .ToListAsync();
        }

        public async Task<Comment> AddCommentAsync(CommentDto commentDto)
        {
            var comment = new Comment
            {
                ClientId = commentDto.ClientId,
                PostId = commentDto.PostId,
                Content = commentDto.Content,
                Date = commentDto.Date
            };

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return comment;
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
    }
}