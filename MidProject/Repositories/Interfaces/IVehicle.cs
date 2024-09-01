using MidProject.Models;
using MidProject.Models.Dto.Request;

namespace MidProject.Repositories.Interfaces
{
    public interface IVehicle
    {
        Task<IEnumerable<Vehicle>> GetAllVehicles();
        Task<Vehicle> GetVehicleById(int id);
        Task AddVehicle(VehicleDto vehicleDto);
        Task UpdateVehicle(VehicleDto vehicleDto);
        Task DeleteVehicle(int id);
    }
}