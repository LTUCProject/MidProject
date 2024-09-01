using MidProject.Data;
using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class ChargingStationServices : IChargingStation
    {
        private readonly MidprojectDbContext _context;

        public ChargingStationServices(MidprojectDbContext context) 
        {
            _context = context;
        }

        public Task AddChargingStation(ChargingStationDto chargingStationDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteChargingStation(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ChargingStation>> GetAllChargingStations()
        {
            throw new NotImplementedException();
        }

        public Task<ChargingStation> GetChargingStationById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateChargingStation(ChargingStationDto chargingStationDto)
        {
            throw new NotImplementedException();
        }
    }
}
