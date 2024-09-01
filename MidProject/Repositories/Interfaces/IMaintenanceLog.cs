using MidProject.Models;
using MidProject.Models.Dto.Request;

namespace MidProject.Repositories.Interfaces
{
    public interface IMaintenanceLog
    {
        Task<IEnumerable<MaintenanceLog>> GetAllMaintenanceLogs();
        Task<MaintenanceLog> GetMaintenanceLogById(int id);
        Task AddMaintenanceLog(MaintenanceLogDto maintenanceLogDto);
        Task UpdateMaintenanceLog(MaintenanceLogDto maintenanceLogDto);
        Task DeleteMaintenanceLog(int id);
    }
}