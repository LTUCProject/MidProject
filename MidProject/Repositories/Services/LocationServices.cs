using MidProject.Data;
using MidProject.Models;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class LocationServices : Repository<Location>, ILocation
    {
        private readonly MidprojectDbContext _context;

        public LocationServices(MidprojectDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
