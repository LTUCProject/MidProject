using MidProject.Models;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;

namespace MidProject.Repository.Interfaces
{
    public interface IServicer
    {
        // Service management
        Task<IEnumerable<ServiceInfoDtoResponse>> GetAllServicesAsync();
        Task<ServiceInfoDtoResponse> GetServiceByIdAsync(int serviceId);
        Task CreateServiceAsync(ServiceInfoDto serviceDto);
        Task UpdateServiceAsync(ServiceInfoDto serviceDto);
        Task DeleteServiceAsync(int serviceId);

        // Service requests management
        Task<IEnumerable<ServiceRequestDtoResponse>> GetServiceRequestsAsync(int serviceId);
        Task AddServiceRequestAsync(ServiceRequestDto requestDto);
        Task RemoveServiceRequestAsync(int requestId);

        // Booking management
        Task<IEnumerable<BookingResponseDto>> GetBookingsAsync(int serviceId);
        Task<BookingResponseDto> GetBookingByIdAsync(int bookingId);
        Task AddBookingAsync(BookingDto bookingDto);
        Task RemoveBookingAsync(int bookingId);

        // Vehicle management
        Task<IEnumerable<VehicleDtoResponse>> GetVehiclesAsync(int serviceId);
        Task<VehicleDtoResponse> GetVehicleByIdAsync(int vehicleId);
        Task AddVehicleAsync(VehicleDto vehicleDto);
        Task RemoveVehicleAsync(int vehicleId);

        //// Feedback management
        Task<IEnumerable<FeedbackDtoResponse>> GetFeedbacksAsync(int serviceId);

        Task AddServiceInfoAsync(ServiceInfoDto serviceInfoDto);


    }
}
