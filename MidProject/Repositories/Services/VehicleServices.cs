using MidProject.Data;
using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class VehicleServices : IVehicle
    {
        private readonly MidprojectDbContext _context;

        public VehicleServices(MidprojectDbContext context) 
        {
            _context = context;
        }

        public Task AddVehicle(VehicleDto vehicleDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteVehicle(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Vehicle>> GetAllVehicles()
        {
            throw new NotImplementedException();
        }

        public Task<Vehicle> GetVehicleById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateVehicle(VehicleDto vehicleDto)
        {
            throw new NotImplementedException();
        }
    }
}
