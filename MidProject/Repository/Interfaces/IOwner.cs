using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;

namespace MidProject.Repository.Interfaces
{
    public interface IOwner
    {
        // Charging station and charger management
        Task<IEnumerable<ChargingStationResponseDto>> GetAllChargingStationsAsync(string accountId);
        Task<ChargingStationResponseDto> GetChargingStationByIdAsync(int stationId);
        Task<ChargingStationResponseDto> CreateChargingStationAsync(ChargingStationDto stationDtoRequest, string accountId);
        Task UpdateChargingStationAsync(int stationId, ChargingStationDto stationDtoRequest, string accountId);
        Task DeleteChargingStationAsync(int stationId, string accountId);


        Task<IEnumerable<ChargerResponseDto>> GetChargersAsync(int stationId);
        Task<ChargerResponseDto> GetChargerByIdAsync(int chargerId);
        Task<Charger> CreateChargerAsync(ChargerDto chargerDtoRequest);
        Task UpdateChargerAsync(int chargerId, ChargerDto chargerDtoRequest);
        Task DeleteChargerAsync(int chargerId);

        // Maintenance logs management
        Task<IEnumerable<MaintenanceLogDtoResponse>> GetMaintenanceLogsAsync(int stationId);
        Task<MaintenanceLog> AddMaintenanceLogAsync(MaintenanceLogDto logDtoRequest);
        Task RemoveMaintenanceLogAsync(int logId);



        //// Location management
        //Task<IEnumerable<LocationDtoResponse>> GetAllLocationsAsync();
        //Task<LocationDtoResponse> GetLocationByIdAsync(int locationId);
        //Task AddLocationAsync(LocationDtoRequest locationDtoRequest);
        //Task RemoveLocationAsync(int locationId);


    }

}
