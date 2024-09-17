using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;

namespace MidProject.Repository.Interfaces
{
    public interface IAdmin
    {
        // Client management
        Task<IEnumerable<ClientResponseDto>> GetAllClientsAsync();
        Task<ClientResponseDto> GetClientByIdAsync(int clientId);
        Task DeleteClientAsync(int clientId);


        // Provider management
        Task<IEnumerable<Provider>> GetAllProvidersAsync();
        Task<Provider> GetProviderByIdAsync(int providerId);
        Task DeleteProviderAsync(int providerId);


        // Client subscriptions management
        Task<IEnumerable<ClientSubscriptionResponseDto>> GetClientSubscriptionByIdAsync(int clientId);
        Task RemoveClientSubscriptionAsync(int clientSubscriptionId);

        // Subscription plan management
        Task<IEnumerable<SubscriptionPlanResponseDto>> GetAllSubscriptionPlansAsync();
        Task<SubscriptionPlanResponseDto> GetSubscriptionPlanByIdAsync(int subscriptionPlanId);
        Task DeleteSubscriptionPlanAsync(int subscriptionPlanId);
        Task<SubscriptionPlan> CreateSubscriptionPlanAsync(SubscriptionPlanDto newSubscriptionPlanDto);


        // Notifications management
        Task<IEnumerable<NotificationResponseDto>> GetClientNotificationsAsync(int clientId);
        Task AddNotificationAsync(NotificationDto notificationDto);

        // Charging station management
        Task<IEnumerable<ChargingStationResponseAdminDto>> GetAllChargingStationsAsync();
        Task<ChargingStationResponseAdminDto> GetChargingStationByIdAsync(int chargingStationId);
        Task DeleteChargingStationAsync(int chargingStationId);

        // Vehicle management
        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
        Task<Vehicle> GetVehicleByIdAsync(int vehicleId);
        Task DeleteVehicleAsync(int vehicleId);

        // Booking management
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking> GetBookingByIdAsync(int bookingId);
        Task DeleteBookingAsync(int bookingId);

        // Location management
        Task<IEnumerable<Location>> GetAllLocationsAsync();
        Task<Location> GetLocationByIdAsync(int locationId);
        Task CreateLocationAsync(Location location);
        Task UpdateLocationAsync(Location location);
        Task DeleteLocationAsync(int locationId);

        // Post management
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Post> GetPostByIdAsync(int postId);
        Task DeletePostAsync(int postId);

        // Comment management
        Task<IEnumerable<Comment>> GetAllCommentsAsync();
        Task<Comment> GetCommentByIdAsync(int commentId);
        Task DeleteCommentAsync(int commentId);
    }
}
