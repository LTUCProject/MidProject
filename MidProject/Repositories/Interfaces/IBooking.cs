using MidProject.Models;
using MidProject.Models.Dto.Request;

namespace MidProject.Repositories.Interfaces
{
    public interface IBooking
    {
        Task<IEnumerable<Booking>> GetAllBookings();
        Task<Booking> GetBookingById(int id);
        Task AddBooking(BookingDto bookingDto);
        Task UpdateBooking(BookingDto bookingDto);
        Task DeleteBooking(int id);
    }
}