using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;

namespace MidProject.Repository.Interfaces
{
    public interface IClient
    {
        //ChargingStation Management
        Task<IEnumerable<ChargingStationResponseAdminDto>> GetAllChargingStationsAsync();

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
        Task<IEnumerable<FavoriteChargingStationResponseDto>> GetClientChargingStationFavoritesAsync();
        Task<IEnumerable<FavoriteServiceInfoResponseDto>> GetClientServiceInfoFavoritesAsync();
        Task<FavoriteChargingStationResponseDto> AddChargingStationFavoriteAsync(FavoriteChargingStationDto favoriteDto);
        Task<FavoriteServiceInfoResponseDto> AddServiceInfoFavoriteAsync(FavoriteServiceInfoDto favoriteDto);
        Task RemoveChargingStationFavoriteAsync(int favoriteId);
        Task RemoveServiceInfoFavoriteAsync(int favoriteId);

        // Vehicle management
        Task<IEnumerable<VehicleResponseDto>> GetClientVehiclesAsync(string accountId);
        Task<Vehicle> GetVehicleByIdAsync(int vehicleId);
        Task<Vehicle> AddVehicleAsync(VehicleDto vehicleDto, string accountId);
        Task RemoveVehicleAsync(int vehicleId);

        // Booking management
        Task<IEnumerable<BookingDto>> GetClientBookingsAsync();
        Task<BookingDto> GetBookingByIdAsync(int bookingId);
        Task<BookingDto> AddBookingAsync(ClientBookingDto bookingDto);
        Task RemoveBookingAsync(int bookingId);

        // Service request management
        Task<IEnumerable<ServiceRequestResponseDTO>> GetClientServiceRequestsAsync();

        Task<ServiceRequest> GetServiceRequestByIdAsync(int requestId);

        Task<IEnumerable<ClinetServiceInfoResponseDto>> GetAllServiceInfosAsync();

        Task<ServiceRequestDtoResponse> CreateServiceRequestAsync(ClientServiceRequestDto requestDto);

        Task DeleteServiceRequestAsync(int requestId);

        // Feedback management
        Task<IEnumerable<Feedback>> GetAllFeedbacksAsync();
        Task<IEnumerable<Feedback>> GetFeedbacksByServiceInfoIdAsync(int serviceInfoId);

        Task<IEnumerable<Feedback>> GetClientFeedbacksAsync(int clientId);
        Task<Feedback>AddFeedbackAsync(FeedbackDto feedbackDto);
        Task RemoveFeedbackAsync(int feedbackId);

        // Notifications management
        Task<IEnumerable<Notification>> GetClientNotificationsAsync();
        Task<Notification> GetNotificationByIdAsync(int notificationId);
        Task DeleteNotificationAsync(int notificationId);

        // Post management
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<PostResponseDto> AddPostAsync(PostDto postDto);
        Task<PostResponseDto> UpdatePostByIdAsync(int postId, PostDto postDto);
        Task DeletePostAsync(int postId);

        // Comment management
        Task<IEnumerable<CommentResponseDto>> GetAllCommentsAsync();
        Task<CommentResponseDto> AddCommentAsync(CommentDto commentDto);
        Task<CommentResponseDto> UpdateCommentByIdAsync(int commentId, CommentDto commentDto);
        Task DeleteCommentAsync(int commentId);

        // ClientSubscription management
        Task<IEnumerable<ClientSubscriptionResponseDto>> GetClientSubscriptionsAsync();
        Task<ClientSubscription> AddSubscriptionAsync(ClientSubscriptionDto subscriptionDto);
        Task RemoveSubscriptionAsync(int clientSubscriptionId);
        Task<IEnumerable<SubscriptionPlanResponseDto>> GetAvailableSubscriptionPlansAsync();

    }
}