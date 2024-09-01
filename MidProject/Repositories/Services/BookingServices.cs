using Microsoft.EntityFrameworkCore;
using MidProject.Data;
using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Repositories.Interfaces;
using MidProject.Repositories.Services;

namespace MidProject.Repositories.Services
{
    public class BookingServices : IBooking
    { 
        private readonly MidprojectDbContext _context;

        public BookingServices(MidprojectDbContext context) 
        {
            _context = context;
        }

        public Task AddBooking(BookingDto bookingDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteBooking(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Booking>> GetAllBookings()
        {
            throw new NotImplementedException();
        }

        public Task<Booking> GetBookingById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateBooking(BookingDto bookingDto)
        {
            throw new NotImplementedException();
        }
    }
}

