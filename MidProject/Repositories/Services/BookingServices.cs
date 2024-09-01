using Microsoft.EntityFrameworkCore;
using MidProject.Data;
using MidProject.Models;
using MidProject.Repositories.Interfaces;
using MidProject.Repositories.Services;

namespace MidProject.Repositories.Services
{
    public class BookingServices : Repository<Booking>, IBooking
    { 
        private readonly MidprojectDbContext _context;

        public BookingServices(MidprojectDbContext context) : base(context)
        {
            _context = context;
        }

    }
}

