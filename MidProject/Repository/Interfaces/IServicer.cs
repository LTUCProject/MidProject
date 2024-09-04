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
        Task CreateServiceAsync(ServiceInfoDtoRequest serviceDto);
        Task UpdateServiceAsync(ServiceInfoDtoRequest serviceDto);
        Task DeleteServiceAsync(int serviceId);

        // Service requests management
        Task<IEnumerable<ServiceRequestDtoResponse>> GetServiceRequestsAsync(int serviceId);
        Task AddServiceRequestAsync(ServiceRequestDtoRequest requestDto);
        Task RemoveServiceRequestAsync(int requestId);

        //// Booking management
        //Task<IEnumerable<Booking>> GetBookingsAsync(int serviceId);
        //Task<Booking> GetBookingByIdAsync(int bookingId);
        //Task AddBookingAsync(Booking booking);
        //Task RemoveBookingAsync(int bookingId);

        //// Vehicle management
        //Task<IEnumerable<Vehicle>> GetVehiclesAsync(int serviceId);
        //Task<Vehicle> GetVehicleByIdAsync(int vehicleId);
        //Task AddVehicleAsync(Vehicle vehicle);
        //Task RemoveVehicleAsync(int vehicleId);

        //// Feedback management
        //Task<IEnumerable<Feedback>> GetFeedbacksAsync(int serviceId);
        //Task AddFeedbackAsync(Feedback feedback);
        //Task RemoveFeedbackAsync(int feedbackId);
    }
}
