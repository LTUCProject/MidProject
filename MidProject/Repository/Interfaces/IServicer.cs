using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;

namespace MidProject.Repository.Interfaces
{
    public interface IServicer
    {
        // Service management
        Task<ServiceInfoResponseDto> CreateServiceInfoAsync(ServiceInfoRequestDto serviceInfoDto);
        Task<ServiceInfoResponseDto> GetServiceInfoByIdAsync(int serviceInfoId);
        Task<IEnumerable<ServiceInfoResponseDto>> GetAllServiceInfosAsync();
        Task<bool> UpdateServiceInfoAsync(int serviceInfoId, ServiceInfoRequestDto serviceInfoDto);
        Task<bool> DeleteServiceInfoAsync(int serviceInfoId);

        // Service requests management
        Task<ServiceRequestDto> CreateServiceRequestAsync(ServiceRequestDto serviceRequestDto);
        Task<ServiceRequestDto> GetServiceRequestByIdAsync(int serviceRequestId);
        Task<IEnumerable<ServiceRequestDto>> GetServiceRequestsByServiceInfoIdAsync(int serviceInfoId);
        Task<bool> UpdateServiceRequestStatusAsync(int serviceRequestId, string status);
        Task<bool> DeleteServiceRequestAsync(int serviceRequestId);

        // Booking management
        Task<BookingResponseDto2> CreateBookingAsync(BookingRequestDto bookingDto);
        Task<BookingResponseDto2> GetBookingByIdAsync(int bookingId);
        Task<IEnumerable<BookingResponseDto2>> GetBookingsByClientIdAsync(int clientId);
        Task<IEnumerable<BookingResponseDto2>> GetBookingsByServiceInfoIdAsync(int serviceInfoId);
        Task<bool> UpdateBookingStatusAsync(int bookingId, string status);
        Task<bool> CancelBookingAsync(int bookingId);
    }
}
        //// Vehicle management
        //Task<IEnumerable<VehicleDtoResponse>> GetVehiclesAsync(int serviceId);
        //Task<VehicleDtoResponse> GetVehicleByIdAsync(int vehicleId);
        //Task AddVehicleAsync(VehicleDto vehicleDto);
        //Task RemoveVehicleAsync(int vehicleId);

        ////// Feedback management
        //Task<IEnumerable<FeedbackDtoResponse>> GetFeedbacksAsync(int serviceId);

       