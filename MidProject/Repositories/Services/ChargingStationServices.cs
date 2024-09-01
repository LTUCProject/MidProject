using MidProject.Data;
using MidProject.Models;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class ChargingStationServices : Repository<ChargingStation>, IChargingStation
    {
        private readonly MidprojectDbContext _context;

        public ChargingStationServices(MidprojectDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
