using MidProject.Models;
using MidProject.Models.Dto.Request;

namespace MidProject.Repositories.Interfaces
{
    public interface IChargingStation
    {
        Task<IEnumerable<ChargingStation>> GetAllChargingStations();
        Task<ChargingStation> GetChargingStationById(int id);
        Task AddChargingStation(ChargingStationDto chargingStationDto);
        Task UpdateChargingStation(ChargingStationDto chargingStationDto);
        Task DeleteChargingStation(int id);
    }
}