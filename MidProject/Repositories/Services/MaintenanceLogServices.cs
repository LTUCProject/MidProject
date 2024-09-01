using MidProject.Data;
using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class MaintenanceLogServices : IMaintenanceLog
    {
        private readonly MidprojectDbContext _context;

        public MaintenanceLogServices(MidprojectDbContext context) 
        {
            _context = context;
        }

        public Task AddMaintenanceLog(MaintenanceLogDto maintenanceLogDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMaintenanceLog(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MaintenanceLog>> GetAllMaintenanceLogs()
        {
            throw new NotImplementedException();
        }

        public Task<MaintenanceLog> GetMaintenanceLogById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateMaintenanceLog(MaintenanceLogDto maintenanceLogDto)
        {
            throw new NotImplementedException();
        }
    }
    
}
