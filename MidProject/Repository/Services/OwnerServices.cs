using MidProject.Models;
using MidProject.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MidProject.Data;
using MidProject.Models.Dto.Response;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Request;
using System;

namespace MidProject.Repository.Services
{
    public class OwnerServices : IOwner
    {
        private readonly MidprojectDbContext _context;

        public OwnerServices(MidprojectDbContext context)
        {
            _context = context;
        }

        // Charging Station Management
        public async Task<IEnumerable<ChargingStationResponseDto>> GetAllChargingStationsAsync(string accountId)
        {
            try
            {
                var owner = await _context.Providers
                    .Include(p => p.ChargingStations)
                    .FirstOrDefaultAsync(p => p.AccountId == accountId);

                if (owner == null)
                {
                    throw new UnauthorizedAccessException("Owner not found");
                }

                var stationDtos = owner.ChargingStations.Select(cs => new ChargingStationResponseDto
                {
                    ChargingStationId = cs.ChargingStationId,
                    StationLocation = cs.StationLocation,
                    LocationId = cs.LocationId,
                    Name = cs.Name,
                    HasParking = cs.HasParking,
                    Status = cs.Status,
                    PaymentMethod = cs.PaymentMethod
                });

                return stationDtos;
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("An error occurred while retrieving charging stations.", ex);
            }
        }

        public async Task<ChargingStationResponseDto> GetChargingStationByIdAsync(int stationId)
        {
            try
            {
                var station = await _context.ChargingStations
                    .Include(cs => cs.Provider)
                    .FirstOrDefaultAsync(cs => cs.ChargingStationId == stationId);

                if (station == null) return null;

                return new ChargingStationResponseDto
                {
                    ChargingStationId = station.ChargingStationId,
                    StationLocation = station.StationLocation,
                    LocationId = station.LocationId,
                    Name = station.Name,
                    HasParking = station.HasParking,
                    Status = station.Status,
                    PaymentMethod = station.PaymentMethod
                };
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("An error occurred while retrieving the charging station by ID.", ex);
            }
        }

        public async Task<ChargingStationResponseDto> CreateChargingStationAsync(ChargingStationDto stationDtoRequest, string accountId)
        {
            try
            {
                var owner = await _context.Providers.FirstOrDefaultAsync(p => p.AccountId == accountId);

                if (owner == null)
                {
                    throw new UnauthorizedAccessException("Owner not found");
                }

                var station = new ChargingStation
                {
                    StationLocation = stationDtoRequest.StationLocation,
                    LocationId = stationDtoRequest.LocationId,
                    Name = stationDtoRequest.Name,
                    HasParking = stationDtoRequest.HasParking,
                    Status = stationDtoRequest.Status,
                    PaymentMethod = stationDtoRequest.PaymentMethod,
                    ProviderId = owner.ProviderId // Set the owner as the provider
                };

                await _context.ChargingStations.AddAsync(station);
                await _context.SaveChangesAsync();

                return new ChargingStationResponseDto
                {
                    ChargingStationId = station.ChargingStationId,
                    StationLocation = station.StationLocation,
                    LocationId = station.LocationId,
                    Name = station.Name,
                    HasParking = station.HasParking,
                    Status = station.Status,
                    PaymentMethod = station.PaymentMethod
                };
            }
            catch (DbUpdateException ex)
            {
                // Log the error
                throw new Exception("An error occurred while creating the charging station.", ex);
            }
        }

        public async Task UpdateChargingStationAsync(int stationId, ChargingStationDto stationDtoRequest, string accountId)
        {
            try
            {
                var owner = await _context.Providers.FirstOrDefaultAsync(p => p.AccountId == accountId);
                var station = await _context.ChargingStations.FindAsync(stationId);

                if (owner == null || station == null || station.ProviderId != owner.ProviderId)
                {
                    throw new UnauthorizedAccessException("Access denied or station not found");
                }

                station.StationLocation = stationDtoRequest.StationLocation;
                station.LocationId = stationDtoRequest.LocationId;
                station.Name = stationDtoRequest.Name;
                station.HasParking = stationDtoRequest.HasParking;
                station.Status = stationDtoRequest.Status;
                station.PaymentMethod = stationDtoRequest.PaymentMethod;

                _context.ChargingStations.Update(station);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the error
                throw new Exception("An error occurred while updating the charging station.", ex);
            }
        }

        public async Task DeleteChargingStationAsync(int stationId, string accountId)
        {
            try
            {
                var owner = await _context.Providers.FirstOrDefaultAsync(p => p.AccountId == accountId);
                var station = await _context.ChargingStations.FindAsync(stationId);

                if (owner == null || station == null || station.ProviderId != owner.ProviderId)
                {
                    throw new UnauthorizedAccessException("Access denied or station not found");
                }

                _context.ChargingStations.Remove(station);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the error
                throw new Exception("An error occurred while deleting the charging station.", ex);
            }
        }

        // Charger Management
        public async Task<IEnumerable<ChargerResponseDto>> GetChargersAsync(int stationId)
        {
            try
            {
                var chargers = await _context.Chargers
                    .Where(c => c.ChargingStationId == stationId)
                    .ToListAsync();

                return chargers.Select(c => new ChargerResponseDto
                {
                    ChargerId = c.ChargerId,
                    Type = c.Type,
                    Power = c.Power,
                    Speed = c.Speed,
                    ChargingStationId = c.ChargingStationId
                });
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("An error occurred while retrieving chargers.", ex);
            }
        }

        public async Task<ChargerResponseDto> GetChargerByIdAsync(int chargerId)
        {
            try
            {
                var charger = await _context.Chargers
                    .FirstOrDefaultAsync(c => c.ChargerId == chargerId);

                if (charger == null) return null;

                return new ChargerResponseDto
                {
                    ChargerId = charger.ChargerId,
                    Type = charger.Type,
                    Power = charger.Power,
                    Speed = charger.Speed,
                    ChargingStationId = charger.ChargingStationId
                };
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("An error occurred while retrieving the charger by ID.", ex);
            }
        }

        public async Task CreateChargerAsync(ChargerDto chargerDtoRequest)
        {
            try
            {
                var charger = new Charger
                {
                    Type = chargerDtoRequest.Type,
                    Power = chargerDtoRequest.Power,
                    Speed = chargerDtoRequest.Speed,
                    ChargingStationId = chargerDtoRequest.ChargingStationId
                };

                await _context.Chargers.AddAsync(charger);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the error
                throw new Exception("An error occurred while creating the charger.", ex);
            }
        }

        public async Task UpdateChargerAsync(int chargerId, ChargerDto chargerDtoRequest)
        {
            try
            {
                var charger = await _context.Chargers.FindAsync(chargerId);

                if (charger == null) return;

                charger.Type = chargerDtoRequest.Type;
                charger.Power = chargerDtoRequest.Power;
                charger.Speed = chargerDtoRequest.Speed;
                charger.ChargingStationId = chargerDtoRequest.ChargingStationId;

                _context.Chargers.Update(charger);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the error
                throw new Exception("An error occurred while updating the charger.", ex);
            }
        }

        public async Task DeleteChargerAsync(int chargerId)
        {
            try
            {
                var charger = await _context.Chargers.FindAsync(chargerId);

                if (charger != null)
                {
                    _context.Chargers.Remove(charger);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                // Log the error
                throw new Exception("An error occurred while deleting the charger.", ex);
            }
        }

        // Maintenance Logs Management
        public async Task<IEnumerable<MaintenanceLogDtoResponse>> GetMaintenanceLogsAsync(int stationId)
        {
            try
            {
                var logs = await _context.MaintenanceLogs
                    .Where(m => m.ChargingStationId == stationId)
                    .ToListAsync();

                return logs.Select(m => new MaintenanceLogDtoResponse
                {
                    MaintenanceLogId = m.MaintenanceLogId,
                    ChargingStationId = m.ChargingStationId,
                    MaintenanceDate = m.MaintenanceDate,
                    PerformedBy = m.PerformedBy,
                    Details = m.Details,
                    Cost = m.Cost
                });
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("An error occurred while retrieving maintenance logs.", ex);
            }
        }

        public async Task AddMaintenanceLogAsync(MaintenanceLogDto logDtoRequest)
        {
            try
            {
                var log = new MaintenanceLog
                {
                    ChargingStationId = logDtoRequest.ChargingStationId,
                    MaintenanceDate = logDtoRequest.MaintenanceDate,
                    PerformedBy = logDtoRequest.PerformedBy,
                    Details = logDtoRequest.Details,
                    Cost = logDtoRequest.Cost
                };

                await _context.MaintenanceLogs.AddAsync(log);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the error
                throw new Exception("An error occurred while adding a maintenance log.", ex);
            }
        }

        public async Task RemoveMaintenanceLogAsync(int logId)
        {
            try
            {
                var log = await _context.MaintenanceLogs.FindAsync(logId);

                if (log != null)
                {
                    _context.MaintenanceLogs.Remove(log);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                // Log the error
                throw new Exception("An error occurred while deleting the maintenance log.", ex);
            }
        }

        

    }
}
