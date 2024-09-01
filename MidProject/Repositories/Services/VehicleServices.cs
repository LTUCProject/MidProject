using MidProject.Data;
using MidProject.Models;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class VehicleServices : Repository<Vehicle>, IVehicle
    {
        private readonly MidprojectDbContext _context;

        public VehicleServices(MidprojectDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
