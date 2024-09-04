using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;

namespace MidProject.Repository.Interfaces
{
    public interface IOwner
    {
        // Charging station and charger management
        Task<IEnumerable<ChargingStationDtoResponse>> GetAllChargingStationsAsync(string accountId);
        Task<ChargingStationDtoResponse> GetChargingStationByIdAsync(int stationId);
        Task<ChargingStationDtoResponse> CreateChargingStationAsync(ChargingStationDtoRequest stationDtoRequest, string accountId);
        Task UpdateChargingStationAsync(int stationId, ChargingStationDtoRequest stationDtoRequest, string accountId);
        Task DeleteChargingStationAsync(int stationId, string accountId);


        Task<IEnumerable<ChargerDtoResponse>> GetChargersAsync(int stationId);
        Task<ChargerDtoResponse> GetChargerByIdAsync(int chargerId);
        Task CreateChargerAsync(ChargerDtoRequest chargerDtoRequest);
        Task UpdateChargerAsync(int chargerId, ChargerDtoRequest chargerDtoRequest);
        Task DeleteChargerAsync(int chargerId);

        // Maintenance logs management
        Task<IEnumerable<MaintenanceLogDtoResponse>> GetMaintenanceLogsAsync(int stationId);
        Task AddMaintenanceLogAsync(MaintenanceLogDtoRequest logDtoRequest);
        Task RemoveMaintenanceLogAsync(int logId);

 

        //// Location management
        //Task<IEnumerable<LocationDtoResponse>> GetAllLocationsAsync();
        //Task<LocationDtoResponse> GetLocationByIdAsync(int locationId);
        //Task AddLocationAsync(LocationDtoRequest locationDtoRequest);
        //Task RemoveLocationAsync(int locationId);


    }

}
