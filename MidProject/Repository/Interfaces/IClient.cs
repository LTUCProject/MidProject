using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;

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
        Task<IEnumerable<FavoriteChargingStationResponseDto>> GetClientChargingStationFavoritesAsync(int clientId);
        Task<IEnumerable<FavoriteServiceInfoResponseDto>> GetClientServiceInfoFavoritesAsync(int clientId);
        Task<FavoriteChargingStationResponseDto> AddChargingStationFavoriteAsync(FavoriteChargingStationDto favoriteDto);
        Task<FavoriteServiceInfoResponseDto> AddServiceInfoFavoriteAsync(FavoriteServiceInfoDto favoriteDto);
        Task RemoveChargingStationFavoriteAsync(int favoriteId);
        Task RemoveServiceInfoFavoriteAsync(int favoriteId);

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
        Task<ServiceRequest> CreateServiceRequestAsync(ClientServiceRequestDto requestDto);
        Task DeleteServiceRequestAsync(int requestId);

        // Feedback management
        Task<IEnumerable<Feedback>> GetClientFeedbacksAsync(int clientId);
        Task<Feedback> AddFeedbackAsync(FeedbackDto feedbackDto);
        Task RemoveFeedbackAsync(int feedbackId);

        // Notifications management
        Task<IEnumerable<Notification>> GetClientNotificationsAsync(int clientId);
        Task<Notification> GetNotificationByIdAsync(int notificationId);

        // Post management
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Post> AddPostAsync(PostDto postDto);
        Task DeletePostAsync(int postId);

        // Comment management
        Task<IEnumerable<Comment>> GetAllCommentsAsync();
        Task<Comment> AddCommentAsync(CommentDto commentDto);
        Task DeleteCommentAsync(int commentId);
    }
}