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
using Microsoft.AspNetCore.Http.HttpResults;

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
                    .ThenInclude(cs => cs.Chargers)
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
                    PaymentMethod = cs.PaymentMethod,
                    Chargers = cs.Chargers.Select(c => new ChargerResponseDto
                    {
                        ChargerId = c.ChargerId,
                        Type = c.Type,
                        Power = c.Power,
                        Speed = c.Speed,
                        ChargingStationId = c.ChargingStationId
                    }).ToList()
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
                    .Include(cs => cs.Chargers)
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
                    PaymentMethod = station.PaymentMethod,
                    Chargers = station.Chargers.Select(c => new ChargerResponseDto
                    {
                        ChargerId = c.ChargerId,
                        Type = c.Type,
                        Power = c.Power,
                        Speed = c.Speed,
                        ChargingStationId = c.ChargingStationId
                    }).ToList()
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

        public async Task<Charger> CreateChargerAsync(ChargerDto chargerDtoRequest)
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
            return charger;

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

        public async Task<MaintenanceLog> AddMaintenanceLogAsync(MaintenanceLogDto logDtoRequest)
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
            return log;

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


        //Notification

        public async Task<NotificationResponseDto> CreateNotificationAsync(NotificationDto notificationDto)
        {
            var notification = new Notification
            {
                ClientId = notificationDto.ClientId, // Ensure this is correct and matches the Client entity type
                Title = notificationDto.Title,
                Message = notificationDto.Message,
                Date = notificationDto.Date
            };

            await _context.Notifications.AddAsync(notification); // Add the notification to the context
            await _context.SaveChangesAsync(); // Save changes to the database

            // Return the created notification's details
            return new NotificationResponseDto
            {
                NotificationId = notification.NotificationId, // ID generated by the database
                ClientId = notification.ClientId,
                Title = notification.Title,
                Message = notification.Message,
                Date = notification.Date
            };
        }


        public async Task<NotificationResponseDto> GetNotificationByIdAsync(int notificationId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.NotificationId == notificationId);

            if (notification == null)
            {
                return null;
            }

            return new NotificationResponseDto
            {
                NotificationId = notification.NotificationId,
                ClientId = notification.ClientId,
                Title = notification.Title,
                Message = notification.Message,
                Date = notification.Date
            };
        }

        public async Task<IEnumerable<NotificationResponseDto>> GetNotificationsByClientIdAsync(int clientId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.ClientId == clientId)
                .ToListAsync();

            return notifications.Select(n => new NotificationResponseDto
            {
                NotificationId = n.NotificationId,
                ClientId = n.ClientId,
                Title = n.Title,
                Message = n.Message,
                Date = n.Date
            });
        }
        //Location
        public async Task<IEnumerable<LocationResponseDto>> GetAllLocationsAsync()
        {
            return await _context.Locations
                .Select(loc => new LocationResponseDto
                {
                    LocationId = loc.LocationId,
                    Name = loc.Name,
                    Address = loc.Address,
                    Latitude = loc.Latitude,
                    Longitude = loc.Longitude,
                    ChargingStations = loc.ChargingStations.Select(cs => new ChargingStationResponseDto
                    {
                        ChargingStationId = cs.ChargingStationId,
                        StationLocation = cs.StationLocation,
                        LocationId = cs.LocationId,
                        Name = cs.Name,
                        HasParking = cs.HasParking,
                        Status = cs.Status,
                        PaymentMethod = cs.PaymentMethod
                    })
                }).ToListAsync();
        }

        public async Task<LocationResponseDto> GetLocationByIdAsync(int id)
        {
            return await _context.Locations
                .Where(loc => loc.LocationId == id)
                .Select(loc => new LocationResponseDto
                {
                    LocationId = loc.LocationId,
                    Name = loc.Name,
                    Address = loc.Address,
                    Latitude = loc.Latitude,
                    Longitude = loc.Longitude,
                    ChargingStations = loc.ChargingStations.Select(cs => new ChargingStationResponseDto
                    {
                        // Map ChargingStation properties here
                    })
                }).FirstOrDefaultAsync();
        }

        public async Task<LocationResponseDto> CreateLocationAsync(LocationDto locationDto)
        {
            var location = new Location
            {
                Name = locationDto.Name,
                Address = locationDto.Address,
                Latitude = locationDto.Latitude,
                Longitude = locationDto.Longitude
            };

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            return new LocationResponseDto
            {
                LocationId = location.LocationId,
                Name = location.Name,
                Address = location.Address,
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };
        }

        public async Task UpdateLocationAsync(int id, LocationDto locationDto)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null) throw new KeyNotFoundException("Location not found.");

            location.Name = locationDto.Name;
            location.Address = locationDto.Address;
            location.Latitude = locationDto.Latitude;
            location.Longitude = locationDto.Longitude;

            _context.Locations.Update(location);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLocationAsync(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null) throw new KeyNotFoundException("Location not found.");

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
        }
    }

}
