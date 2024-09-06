using System.Linq;
using Microsoft.EntityFrameworkCore;
using MidProject.Data;
using MidProject.Models;
using MidProject.Models.Dto.Request2;
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
        public async Task<IEnumerable<Favorite>> GetClientFavoritesAsync(int clientId)
        {
            return await _context.Favorites
                .Where(f => f.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<Favorite> AddFavoriteAsync(FavoriteDto favoriteDto)
        {
            var favorite = new Favorite
            {
                ServiceInfoId = favoriteDto.ServiceInfoId,
                ChargingStationId = favoriteDto.ChargingStationId,
                ClientId = favoriteDto.ClientId
            };

            await _context.Favorites.AddAsync(favorite);
            await _context.SaveChangesAsync();
            return favorite;
        }

        public async Task RemoveFavoriteAsync(int favoriteId)
        {
            var favorite = await _context.Favorites.FindAsync(favoriteId);
            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
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
                ClientId = vehicleDto.ClientId,
                ServiceInfoId = vehicleDto.ServiceInfoId
            };

            // Add the vehicle to the database
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            // Return the newly added vehicle
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

        public async Task<ServiceRequest> CreateServiceRequestAsync(ServiceRequestDto requestDto)
        {
            var serviceRequest = new ServiceRequest
            {
                ServiceInfoId = requestDto.ServiceInfoId,
                ClientId = requestDto.ClientId,
                ProviderId = requestDto.ProviderId,
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

        //public async Task AddServiceInfoAsync(ServiceInfoDto serviceInfoDto)
        //{
        //    var serviceInfo = new ServiceInfo
        //    {
        //        Name = serviceInfoDto.Name,
        //        Description = serviceInfoDto.Description,
        //        Contact = serviceInfoDto.Contact,
        //        Type = serviceInfoDto.Type,
        //        ProviderId = serviceInfoDto.ProviderId
        //    };

        //    _context.ServiceInfos.Add(serviceInfo);
        //    await _context.SaveChangesAsync();
        //}
    }
}
