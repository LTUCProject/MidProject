using MidProject.Data;
using MidProject.Models;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class MaintenanceLogServices : Repository<MaintenanceLog>, IMaintenanceLog
    {
        private readonly MidprojectDbContext _context;

        public MaintenanceLogServices(MidprojectDbContext context) : base(context)
        {
            _context = context;
        }

    }
    
}
