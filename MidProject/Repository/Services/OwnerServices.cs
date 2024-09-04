using MidProject.Models;
using MidProject.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MidProject.Data;
using MidProject.Models.Dto.Response;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Request;

namespace MidProject.Repository.Services
{
    public class OwnerServices : IOwner
    {
        private readonly MidprojectDbContext _context;

        public OwnerServices(MidprojectDbContext context)
        {
            _context = context;
        }
        // Charging Station Managemen
        public async Task<IEnumerable<ChargingStationDtoResponse>> GetAllChargingStationsAsync(string accountId)
        {
            var owner = await _context.Providers
                .Include(p => p.ChargingStations)
                .FirstOrDefaultAsync(p => p.AccountId == accountId);

            if (owner == null)
            {
                throw new UnauthorizedAccessException("Owner not found");
            }

            var stationDtos = owner.ChargingStations.Select(cs => new ChargingStationDtoResponse
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

        public async Task<ChargingStationDtoResponse> GetChargingStationByIdAsync(int stationId)
        {
            var station = await _context.ChargingStations
                .Include(cs => cs.Provider)
                .FirstOrDefaultAsync(cs => cs.ChargingStationId == stationId);

            if (station == null) return null;

            return new ChargingStationDtoResponse
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

        public async Task<ChargingStationDtoResponse> CreateChargingStationAsync(ChargingStationDtoRequest stationDtoRequest, string accountId)
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

            return new ChargingStationDtoResponse
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

        public async Task UpdateChargingStationAsync(int stationId, ChargingStationDtoRequest stationDtoRequest, string accountId)
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

        public async Task DeleteChargingStationAsync(int stationId, string accountId)
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

        // Charger Management
        public async Task<IEnumerable<ChargerDtoResponse>> GetChargersAsync(int stationId)
        {
            var chargers = await _context.Chargers
                .Where(c => c.ChargingStationId == stationId)
                .ToListAsync();

            return chargers.Select(c => new ChargerDtoResponse
            {
                ChargerId = c.ChargerId,
                Type = c.Type,
                Power = c.Power,
                Speed = c.Speed,
                ChargingStationId = c.ChargingStationId
            });
        }

        public async Task<ChargerDtoResponse> GetChargerByIdAsync(int chargerId)
        {
            var charger = await _context.Chargers
                .FirstOrDefaultAsync(c => c.ChargerId == chargerId);

            if (charger == null) return null;

            return new ChargerDtoResponse
            {
                ChargerId = charger.ChargerId,
                Type = charger.Type,
                Power = charger.Power,
                Speed = charger.Speed,
                ChargingStationId = charger.ChargingStationId
            };
        }

        public async Task CreateChargerAsync(ChargerDtoRequest chargerDtoRequest)
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

        public async Task UpdateChargerAsync(int chargerId, ChargerDtoRequest chargerDtoRequest)
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

        public async Task DeleteChargerAsync(int chargerId)
        {
            var charger = await _context.Chargers.FindAsync(chargerId);

            if (charger != null)
            {
                _context.Chargers.Remove(charger);
                await _context.SaveChangesAsync();
            }
        }

        // Maintenance Logs Management
        public async Task<IEnumerable<MaintenanceLogDtoResponse>> GetMaintenanceLogsAsync(int stationId)
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

        public async Task AddMaintenanceLogAsync(MaintenanceLogDtoRequest logDtoRequest)
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

        public async Task RemoveMaintenanceLogAsync(int logId)
        {
            var log = await _context.MaintenanceLogs.FindAsync(logId);

            if (log != null)
            {
                _context.MaintenanceLogs.Remove(log);
                await _context.SaveChangesAsync();
            }
        }

        //// Location Management
        //public async Task<IEnumerable<LocationDtoResponse>> GetAllLocationsAsync()
        //{
        //    var locations = await _context.Locations.ToListAsync();
        //    return locations.Select(location => new LocationDtoResponse
        //    {
        //        LocationId = location.LocationId,
        //        Name = location.Name,
        //        Address = location.Address,
        //        Latitude = location.Latitude,
        //        Longitude = location.Longitude
        //    }).ToList();
        //}

        //public async Task<LocationDtoResponse> GetLocationByIdAsync(int locationId)
        //{
        //    var location = await _context.Locations.FindAsync(locationId);
        //    if (location == null) return null;

        //    return new LocationDtoResponse
        //    {
        //        LocationId = location.LocationId,
        //        Name = location.Name,
        //        Address = location.Address,
        //        Latitude = location.Latitude,
        //        Longitude = location.Longitude
        //    };
        //}

        //public async Task AddLocationAsync(LocationDtoRequest locationDtoRequest)
        //{
        //    var location = new Location
        //    {
        //        LocationId = locationDtoRequest.LocationId,
        //        Name = locationDtoRequest.Name,
        //        Address = locationDtoRequest.Address,
        //        Latitude = locationDtoRequest.Latitude,
        //        Longitude = locationDtoRequest.Longitude
        //    };

        //    _context.Locations.Add(location);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task RemoveLocationAsync(int locationId)
        //{
        //    var location = await _context.Locations.FindAsync(locationId);
        //    if (location != null)
        //    {
        //        _context.Locations.Remove(location);
        //        await _context.SaveChangesAsync();
        //    }
        //}

    }
}
