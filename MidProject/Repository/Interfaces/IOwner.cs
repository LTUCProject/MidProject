using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;
using System.Threading.Tasks;

namespace MidProject.Repository.Interfaces
{
    public interface IOwner
    {
        // Charging station and charger management
        Task<IEnumerable<ChargingStationResponseDto>> GetAllChargingStationsAsync(string accountId);
        Task<ChargingStationResponseDto> GetChargingStationByIdAsync(int stationId);
        Task<ChargingStationResponseDto> CreateChargingStationAsync(ChargingStationDto stationDtoRequest, string accountId);
        Task UpdateChargingStationAsync(int stationId, ChargingStationDto stationDtoRequest, string accountId);
        Task DeleteChargingStationAsync(int stationId, string accountId);


        Task<IEnumerable<ChargerResponseDto>> GetChargersAsync(int stationId);
        Task<ChargerResponseDto> GetChargerByIdAsync(int chargerId);
        Task<Charger> CreateChargerAsync(ChargerDto chargerDtoRequest);
        Task UpdateChargerAsync(int chargerId, ChargerDto chargerDtoRequest);
        Task DeleteChargerAsync(int chargerId);

        // Maintenance logs management
        Task<IEnumerable<MaintenanceLogDtoResponse>> GetMaintenanceLogsAsync(int stationId);
        Task<MaintenanceLog> AddMaintenanceLogAsync(MaintenanceLogDto logDtoRequest);
        Task RemoveMaintenanceLogAsync(int logId);

        // Notification management
        Task<NotificationResponseDto> CreateNotificationAsync(NotificationDto notificationDto);
        Task<NotificationResponseDto> GetNotificationByIdAsync(int notificationId);
        Task<IEnumerable<NotificationResponseDto>> GetNotificationsByClientIdAsync(int clientId);

        //// Location management
        //Task<IEnumerable<LocationResponseDto>> GetAllLocationsAsync();
        //Task<LocationResponseDto> GetLocationByIdAsync(int id);
        //Task<LocationResponseDto> CreateLocationAsync(LocationDto locationDto);
        //Task UpdateLocationAsync(int id, LocationDto locationDto);
        //Task DeleteLocationAsync(int id);


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

        // Booking management
        Task<IEnumerable<BookingDto>> GetBookingsByChargingStationAsync(int stationId);
        Task<BookingDto> GetBookingByIdAsync(int bookingId);
        Task<IEnumerable<BookingDto>> GetBookingsByDateRangeAsync(int stationId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<BookingDto>> GetPendingBookingsByChargingStationAsync(int stationId);
        Task UpdateBookingDetailsAsync(int bookingId, string newStatus, int newCost); // Combined method for status and cost updates

        // Session management
        Task<IEnumerable<SessionDtoResponse>> GetSessionsByChargingStationAsync(int stationId);
        Task<SessionDtoResponse> GetSessionByIdAsync(int sessionId);
        Task UpdateSessionDetailsAsync(int sessionId, int energyConsumed, int cost);



    }

}
