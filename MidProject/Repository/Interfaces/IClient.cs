﻿using MidProject.Models;
using MidProject.Models.Dto.Request2;

namespace MidProject.Repository.Interfaces
{
    public interface IClient
    {
        // Session management
        Task<IEnumerable<Session>> GetClientSessionsAsync(int clientId);
        Task<Session> GetSessionByIdAsync(int sessionId);
        Task<Session> StartSessionAsync(SessionDto sessionDto);
        Task EndSessionAsync(int sessionId);

        // Payment transactions management
        Task<IEnumerable<PaymentTransaction>> GetClientPaymentsAsync(int sessionId);
        Task<PaymentTransaction> AddPaymentAsync(PaymentTransactionDto paymentDto);
        Task RemovePaymentAsync(int paymentId);

        // Favorites management
        Task<IEnumerable<Favorite>> GetClientFavoritesAsync(int clientId);
        Task<Favorite> AddFavoriteAsync(FavoriteDto favoriteDto);
        Task RemoveFavoriteAsync(int favoriteId);

        // Vehicle management
        Task<IEnumerable<Vehicle>> GetClientVehiclesAsync(int clientId);
        Task<Vehicle> GetVehicleByIdAsync(int vehicleId);
        Task<Vehicle> AddVehicleAsync(VehicleDto vehicleDto);
        Task RemoveVehicleAsync(int vehicleId);

        // Booking management
        Task<IEnumerable<Booking>> GetClientBookingsAsync(int clientId);
        Task<Booking> GetBookingByIdAsync(int bookingId);
        Task<Booking> AddBookingAsync(BookingDto bookingDto);
        Task RemoveBookingAsync(int bookingId);

        // Service request management
        Task<IEnumerable<ServiceRequest>> GetClientServiceRequestsAsync(int clientId);
        Task<ServiceRequest> GetServiceRequestByIdAsync(int requestId);
        Task<ServiceRequest> CreateServiceRequestAsync(ServiceRequestDto requestDto);
        Task DeleteServiceRequestAsync(int requestId);

        // Feedback management
        Task<IEnumerable<Feedback>> GetClientFeedbacksAsync(int clientId);
        Task<Feedback> AddFeedbackAsync(FeedbackDto feedbackDto);
        Task RemoveFeedbackAsync(int feedbackId);

        // Notifications management
        Task<IEnumerable<Notification>> GetClientNotificationsAsync(int clientId);
        Task<Notification> GetNotificationByIdAsync(int notificationId);
      //  Task AddServiceInfoAsync(ServiceInfoDto serviceInfoDto);


    }
}
