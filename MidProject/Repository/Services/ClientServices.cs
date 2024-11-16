using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MidProject.Data;
using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;
using MidProject.Repository.Interfaces;

namespace MidProject.Repository.Services
{
    public class ClientServices : IClient
    {
        private readonly MidprojectDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientServices(MidprojectDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetAccountId()=> _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        

        //ChargingStation Management
         public async Task<IEnumerable<ChargingStationResponseAdminDto>> GetAllChargingStationsAsync()
        {
            return await _context.ChargingStations
                .Include(cs => cs.Chargers)  // Include chargers
                .Include(cs => cs.Provider)  // Include provider
                .Select(cs => new ChargingStationResponseAdminDto
                {
                    ChargingStationId = cs.ChargingStationId,
                    StationLocation = cs.StationLocation,
                    Name = cs.Name,
                    HasParking = cs.HasParking,
                    Status = cs.Status,
                    PaymentMethod = cs.PaymentMethod,

                    // Location properties
                    Address = cs.Address,
                    Latitude = cs.Latitude,
                    Longitude = cs.Longitude,

                    Chargers = cs.Chargers.Select(c => new ChargerResponseDto
                    {
                        ChargerId = c.ChargerId,
                        Type = c.Type,
                        Power = c.Power,
                        Speed = c.Speed,
                        ChargingStationId = c.ChargingStationId
                    }).ToList(),
                    Provider = new ProviderResponseDto
                    {
                        ProviderId = cs.Provider.ProviderId,
                        Name = cs.Provider.Name,
                        Email = cs.Provider.Email,
                        Type = cs.Provider.Type
                    }
                })
                .ToListAsync();
        }

        // Session management
        public async Task<IEnumerable<Session>> GetClientSessionsAsync(int clientId)
        {
            return await _context.Sessions
                .Where(s => s.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<Session> GetSessionByIdAsync(int sessionId)
        {
            return await _context.Sessions
                .FirstOrDefaultAsync(s => s.SessionId == sessionId);
        }

        public async Task<Session> StartSessionAsync(SessionDto sessionDto)
        {
            var session = new Session
            {
                ClientId = sessionDto.ClientId,
                ChargingStationId = sessionDto.ChargingStationId,
                StartTime = sessionDto.StartTime,
                EndTime = sessionDto.EndTime,
                EnergyConsumed = sessionDto.EnergyConsumed,
                Cost=sessionDto.Cost
            };

            await _context.Sessions.AddAsync(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task EndSessionAsync(int sessionId)
        {
            var session = await _context.Sessions.FindAsync(sessionId);
            if (session != null)
            {
                session.EndTime = DateTime.UtcNow;
                _context.Sessions.Update(session);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Handle the case where the session is not found
                throw new KeyNotFoundException("Session not found.");
            }
        }

        // Payment transactions management
        public async Task<IEnumerable<PaymentTransaction>> GetClientPaymentsAsync(int sessionId)
        {
            return await _context.PaymentTransactions
                .Where(pt => pt.SessionId == sessionId)
                .ToListAsync();
        }

        public async Task<PaymentTransaction> AddPaymentAsync(PaymentTransactionDto paymentDto)
        {
            var payment = new PaymentTransaction
            {
                SessionId = paymentDto.SessionId,
                ClientId = paymentDto.ClientId,
                Amount = paymentDto.Amount,
                PaymentDate = paymentDto.PaymentDate,
                PaymentMethod = paymentDto.PaymentMethod,
                Status = paymentDto.Status
            };

            await _context.PaymentTransactions.AddAsync(payment);
            await _context.SaveChangesAsync();

            return payment;
        }

        public async Task RemovePaymentAsync(int paymentId)
        {
            var payment = await _context.PaymentTransactions.FindAsync(paymentId);
            if (payment != null)
            {
                _context.PaymentTransactions.Remove(payment);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Handle the case where the payment is not found
                throw new KeyNotFoundException("Payment transaction not found.");
            }
        }

        // Favorites management

        // Retrieve all charging station favorites for a client with detailed information
        public async Task<IEnumerable<FavoriteChargingStationResponseDto>> GetClientChargingStationFavoritesAsync()
        {
            try
            {
                // Fetch the accountId from the current user's claims
                var accountId = GetAccountId();

                // Fetch the client based on the accountId
                var client = await _context.Clients.FirstOrDefaultAsync(c => c.AccountId == accountId);

                if (client == null)
                {
                    throw new UnauthorizedAccessException("Client not found");
                }

                // Fetch the favorites for the found client
                var favorites = await _context.ChargingStationFavorites
                    .Where(f => f.ClientId == client.ClientId)  // Use the client.ClientId for filtering
                    .Include(f => f.ChargingStation) // Include the related ChargingStation entity
                    .Select(f => new FavoriteChargingStationResponseDto
                    {
                        FavoriteId = f.ChargingStationFavoriteId,
                        ChargingStationId = f.ChargingStationId,
                        ClientId = f.ClientId,
                        ChargingStationName = f.ChargingStation.Name,
                        StationLocation = f.ChargingStation.StationLocation,
                        HasParking = f.ChargingStation.HasParking,
                        Status = f.ChargingStation.Status,
                        PaymentMethod = f.ChargingStation.PaymentMethod
                    })
                    .ToListAsync();

                return favorites;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving client charging station favorites.", ex);
            }
        }

        // Add a new charging station favorite
        public async Task<FavoriteChargingStationResponseDto> AddChargingStationFavoriteAsync(FavoriteChargingStationDto favoriteDto)
        {
            try
            {
                // Fetch the accountId from the current user's claims
                var accountId = GetAccountId();

                // Fetch the client based on the accountId
                var client = await _context.Clients.FirstOrDefaultAsync(c => c.AccountId == accountId);

                if (client == null)
                {
                    throw new UnauthorizedAccessException("Client not found");
                }

                // Create the new favorite
                var favorite = new ChargingStationFavorite
                {
                    ClientId = client.ClientId,  // Use client.ClientId
                    ChargingStationId = favoriteDto.ChargingStationId
                };

                _context.ChargingStationFavorites.Add(favorite);
                await _context.SaveChangesAsync();

                // Retrieve the details of the charging station just added
                var chargingStation = await _context.ChargingStations
                    .Where(cs => cs.ChargingStationId == favoriteDto.ChargingStationId)
                    .Select(cs => new FavoriteChargingStationResponseDto
                    {
                        FavoriteId = favorite.ChargingStationFavoriteId,
                        ChargingStationId = cs.ChargingStationId,
                        ClientId = client.ClientId,  // Use client.ClientId
                        ChargingStationName = cs.Name,
                        StationLocation = cs.StationLocation,
                        HasParking = cs.HasParking,
                        Status = cs.Status,
                        PaymentMethod = cs.PaymentMethod
                    })
                    .FirstOrDefaultAsync();

                return chargingStation;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding a charging station favorite.", ex);
            }
        }

        // Retrieve all service info favorites for a client with detailed information
        public async Task<IEnumerable<FavoriteServiceInfoResponseDto>> GetClientServiceInfoFavoritesAsync()
        {
            try
            {
                // Fetch the accountId from the current user's claims
                var accountId = GetAccountId();

                // Fetch the client based on the accountId
                var client = await _context.Clients.FirstOrDefaultAsync(c => c.AccountId == accountId);

                if (client == null)
                {
                    throw new UnauthorizedAccessException("Client not found");
                }

                // Fetch the service info favorites for the found client
                var favorites = await _context.ServiceInfoFavorites
                    .Where(f => f.ClientId == client.ClientId)  // Use client.ClientId for filtering
                    .Include(f => f.ServiceInfo) // Include the related ServiceInfo entity
                    .Select(f => new FavoriteServiceInfoResponseDto
                    {
                        FavoriteId = f.ServiceInfoFavoriteId,
                        ServiceInfoId = f.ServiceInfoId,
                        ClientId = f.ClientId,
                        ServiceInfoName = f.ServiceInfo.Name,
                        Description = f.ServiceInfo.Description,
                        Contact = f.ServiceInfo.Contact,
                        Type = f.ServiceInfo.Type
                    })
                    .ToListAsync();

                return favorites;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving client service info favorites.", ex);
            }
        }

        // Add a new service info favorite
        public async Task<FavoriteServiceInfoResponseDto> AddServiceInfoFavoriteAsync(FavoriteServiceInfoDto favoriteDto)
        {
            try
            {
                // Fetch the accountId from the current user's claims
                var accountId = GetAccountId();

                // Fetch the client based on the accountId
                var client = await _context.Clients.FirstOrDefaultAsync(c => c.AccountId == accountId);

                if (client == null)
                {
                    throw new UnauthorizedAccessException("Client not found");
                }

                // Create the new favorite
                var favorite = new ServiceInfoFavorite
                {
                    ClientId = client.ClientId,  // Use client.ClientId
                    ServiceInfoId = favoriteDto.ServiceInfoId
                };

                _context.ServiceInfoFavorites.Add(favorite);
                await _context.SaveChangesAsync();

                // Retrieve the details of the service info just added
                var serviceInfo = await _context.ServiceInfos
                    .Where(si => si.ServiceInfoId == favoriteDto.ServiceInfoId)
                    .Select(si => new FavoriteServiceInfoResponseDto
                    {
                        FavoriteId = favorite.ServiceInfoFavoriteId,
                        ServiceInfoId = si.ServiceInfoId,
                        ClientId = client.ClientId,  // Use client.ClientId
                        ServiceInfoName = si.Name,
                        Description = si.Description,
                        Contact = si.Contact,
                        Type = si.Type
                    })
                    .FirstOrDefaultAsync();

                return serviceInfo;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding a service info favorite.", ex);
            }
        }

        // Remove a charging station favorite
        public async Task RemoveChargingStationFavoriteAsync(int favoriteId)
        {
            try
            {
                var favorite = await _context.ChargingStationFavorites.FindAsync(favoriteId);
                if (favorite != null)
                {
                    _context.ChargingStationFavorites.Remove(favorite);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while removing the charging station favorite.", ex);
            }
        }

        // Remove a service info favorite
        public async Task RemoveServiceInfoFavoriteAsync(int favoriteId)
        {
            try
            {
                var favorite = await _context.ServiceInfoFavorites.FindAsync(favoriteId);
                if (favorite != null)
                {
                    _context.ServiceInfoFavorites.Remove(favorite);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while removing the service info favorite.", ex);
            }
        }


        // Vehicle management
        public async Task<IEnumerable<VehicleResponseDto>> GetClientVehiclesAsync(string accountId)
        {
            try
            {
                // Fetch the client based on the accountId and include related Vehicles
                var client = await _context.Clients
                    .Include(c => c.Vehicles)
                    .FirstOrDefaultAsync(c => c.AccountId == accountId);

                if (client == null)
                {
                    throw new UnauthorizedAccessException("Client not found");
                }

                // Map the vehicles to a DTO
                var vehicleDtos = client.Vehicles.Select(v => new VehicleResponseDto
                {
                    VehicleId = v.VehicleId,
                    LicensePlate = v.LicensePlate,
                    Model = v.Model,
                    Year = v.Year,
                    BatteryCapacity = v.BatteryCapacity,
                    ElectricType = v.ElectricType,
                    ClientId = v.ClientId  // Optional: Remove if sensitive
                }).ToList();

                return vehicleDtos;
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("An error occurred while retrieving client vehicles.", ex);
            }
        }



        public async Task<Vehicle> GetVehicleByIdAsync(int vehicleId)
        {
            return await _context.Vehicles
                .FirstOrDefaultAsync(v => v.VehicleId == vehicleId);
        }

        public async Task<Vehicle> AddVehicleAsync(VehicleDto vehicleDto, string accountId)
        {
            // Retrieve the client using the accountId
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.AccountId == accountId);

            if (client == null)
            {
                throw new UnauthorizedAccessException("Client not found.");
            }

            // Create the vehicle and assign the ClientId from the found client
            var vehicle = new Vehicle
            {
                LicensePlate = vehicleDto.LicensePlate,
                Model = vehicleDto.Model,
                Year = vehicleDto.Year,
                BatteryCapacity = vehicleDto.BatteryCapacity,
                ElectricType = vehicleDto.ElectricType,
                ClientId = client.ClientId  // Automatically assign ClientId
            };

            // Add vehicle to the database and save changes
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return vehicle;
        }

        public async Task RemoveVehicleAsync(int vehicleId)
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
        }


        // Booking management
        public async Task<IEnumerable<BookingDto>> GetClientBookingsAsync()
        {
            try
            {
                // Fetch the accountId from the current user's claims
                var accountId = GetAccountId();

                // Fetch the client based on the accountId
                var client = await _context.Clients.FirstOrDefaultAsync(c => c.AccountId == accountId);

                if (client == null)
                {
                    throw new UnauthorizedAccessException("Client not found");
                }

                // Fetch the bookings for the found client
                var bookings = await _context.Bookings
                    .Where(b => b.ClientId == client.ClientId)
                    .Select(b => new BookingDto
                    {
                        BookingId = b.BookingId,
                        ClientId = b.ClientId,
                        ChargingStationId = b.ChargingStationId,
                        VehicleId = b.VehicleId,
                        StartTime = b.StartTime,
                        EndTime = b.EndTime,
                        Status = b.Status,
                        Cost = b.Cost
                    })
                    .ToListAsync();

                return bookings;
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("An error occurred while retrieving client bookings.", ex);
            }
        }

        public async Task<BookingDto> GetBookingByIdAsync(int bookingId)
        {
            try
            {
                // Fetch the accountId from the current user's claims
                var accountId = GetAccountId();

                // Fetch the client based on the accountId
                var client = await _context.Clients.FirstOrDefaultAsync(c => c.AccountId == accountId);

                if (client == null)
                {
                    throw new UnauthorizedAccessException("Client not found");
                }

                // Fetch the booking by its ID for the authenticated client
                var booking = await _context.Bookings
                    .Where(b => b.BookingId == bookingId && b.ClientId == client.ClientId) // Ensure the booking belongs to the client
                    .Select(b => new BookingDto
                    {
                        BookingId = b.BookingId,
                        ClientId = b.ClientId,
                        ChargingStationId = b.ChargingStationId,
                        VehicleId = b.VehicleId,
                        StartTime = b.StartTime,
                        EndTime = b.EndTime,
                        Status = b.Status,
                        Cost = b.Cost
                    })
                    .FirstOrDefaultAsync();

                if (booking == null)
                {
                    throw new KeyNotFoundException("Booking not found.");
                }

                return booking;
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("An error occurred while retrieving the booking.", ex);
            }
        }

        public async Task<BookingDto> AddBookingAsync(ClientBookingDto bookingDto)
        {
            try
            {
                // Fetch the accountId from the current user's claims
                var accountId = GetAccountId();

                // Fetch the client based on the accountId
                var client = await _context.Clients.FirstOrDefaultAsync(c => c.AccountId == accountId);

                if (client == null)
                {
                    throw new UnauthorizedAccessException("Client not found.");
                }

                // Fetch the vehicle based on the VehicleId from the DTO
                var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.VehicleId == bookingDto.VehicleId);

                if (vehicle == null)
                {
                    throw new UnauthorizedAccessException("Vehicle not found.");
                }

                // Map the DTO to the Booking entity
                var booking = new Booking
                {
                    ClientId = client.ClientId, // Automatically set the ClientId
                    ChargingStationId = bookingDto.ChargingStationId,
                    VehicleId = vehicle.VehicleId, // Automatically set the VehicleId
                    StartTime = bookingDto.StartTime,
                    EndTime = bookingDto.EndTime,
                    Status = bookingDto.Status,  // Status is set from DTO; defaults to "Pending"
                    Cost = bookingDto.Cost       // Cost is set from DTO; defaults to 0
                };

                // Add the booking to the database
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                // Now, map and return the BookingDto with auto-generated fields
                var bookingResponse = new BookingDto
                {
                    BookingId = booking.BookingId,
                    ClientId = client.ClientId,
                    ClientName = client.Name,  // Auto-generate ClientName
                    ClientEmail = client.Email,  // Auto-generate ClientEmail
                    VehicleModel = vehicle.Model, // Auto-generate VehicleModel
                    ChargingStationId = booking.ChargingStationId,
                    VehicleId = booking.VehicleId,
                    StartTime = booking.StartTime,
                    EndTime = booking.EndTime,
                    Status = booking.Status,
                    Cost = booking.Cost
                };

                return bookingResponse; // Return BookingDto, not Booking
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("An error occurred while adding the booking.", ex);
            }
        }




        public async Task RemoveBookingAsync(int bookingId)
        {
            try
            {
                // Fetch the accountId from the current user's claims
                var accountId = GetAccountId();

                // Fetch the client based on the accountId
                var client = await _context.Clients.FirstOrDefaultAsync(c => c.AccountId == accountId);

                if (client == null)
                {
                    throw new UnauthorizedAccessException("Client not found.");
                }

                // Fetch the booking by its ID for the authenticated client
                var booking = await _context.Bookings
                    .Where(b => b.BookingId == bookingId && b.ClientId == client.ClientId) // Ensure the booking belongs to the client
                    .FirstOrDefaultAsync();

                if (booking == null)
                {
                    throw new KeyNotFoundException("Booking not found.");
                }

                // Remove the booking from the database
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("An error occurred while removing the booking.", ex);
            }
        }



        // Service request management
        public async Task<IEnumerable<ServiceRequestResponseDTO>> GetClientServiceRequestsAsync()
        {
            try
            {


                // Fetch the accountId from the current user's claims
                var accountId = GetAccountId();

                // Fetch the client based on the accountId
                var client = await _context.Clients.FirstOrDefaultAsync(c => c.AccountId == accountId);

                if (client == null)
                {
                    throw new UnauthorizedAccessException("Client not found");
                }

                // Fetch the service requests for the found client and map to DTO
                var serviceRequests = await _context.ServiceRequests
                    .Where(sr => sr.ClientId == client.ClientId)
                    .Include(sr => sr.ServiceInfo)  // Include related ServiceInfo
                    .Include(sr => sr.Provider)     // Include related Provider
                    .Include(sr => sr.Vehicle)     // Include related Vehicle
                    .ToListAsync();

                // Map ServiceRequests to ServiceRequestDTOs
                var serviceRequestDTOs = serviceRequests.Select(sr => new ServiceRequestResponseDTO
                {
                    ServiceRequestId = sr.ServiceRequestId,
                    ServiceInfoId = sr.ServiceInfoId,
                    ServiceInfoName = sr.ServiceInfo.Name,  // Assuming Name is a field in ServiceInfo
                    ClientId = sr.ClientId,
                    ClientName = sr.Client.Name,            // Assuming Name is a field in Client
                    ProviderId = sr.ProviderId,
                    ProviderName = sr.Provider.Name,       // Assuming Name is a field in Provider
                    VehicleId = sr.VehicleId,
                    VehicleModel = sr.Vehicle.Model,       // Assuming Model is a field in Vehicle
                    Status = sr.Status
                });

                return serviceRequestDTOs;
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("An error occurred while retrieving client service requests.", ex);
            }
        }


        public async Task<ServiceRequest> GetServiceRequestByIdAsync(int requestId)
        {
            return await _context.ServiceRequests
                .FirstOrDefaultAsync(sr => sr.ServiceRequestId == requestId);
        }

        public async Task<IEnumerable<ClinetServiceInfoResponseDto>> GetAllServiceInfosAsync()
        {
            try
            {
                // Fetch all service info records in the database
                return await _context.ServiceInfos
                    .Include(serviceInfo => serviceInfo.Provider)
                    .Select(serviceInfo => new ClinetServiceInfoResponseDto
                    {
                        Provider = new ProviderResponseDto
                        {
                            ProviderId = serviceInfo.Provider.ProviderId,
                            Name = serviceInfo.Provider.Name,
                            Email = serviceInfo.Provider.Email,
                            Type = serviceInfo.Provider.Type
                        },
                        ServiceInfoId = serviceInfo.ServiceInfoId,
                        Name = serviceInfo.Name,
                        Description = serviceInfo.Description,
                        Contact = serviceInfo.Contact,
                        Type = serviceInfo.Type,
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("An error occurred while retrieving all service information.", ex);
            }
        }

        public async Task<ServiceRequestDtoResponse> CreateServiceRequestAsync(ClientServiceRequestDto requestDto)
        {
            try
            {
                // Fetch the accountId from the current user's claims
                var accountId = GetAccountId();

                // Fetch the client based on the accountId
                var client = await _context.Clients.FirstOrDefaultAsync(c => c.AccountId == accountId);

                if (client == null)
                {
                    throw new UnauthorizedAccessException("Client not found.");
                }

                // Fetch the vehicle based on the VehicleId from the DTO
                var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.VehicleId == requestDto.VehicleId);

                if (vehicle == null)
                {
                    throw new UnauthorizedAccessException("Vehicle not found.");
                }

                // Create a new service request
                var serviceRequest = new ServiceRequest
                {
                    ServiceInfoId = requestDto.ServiceInfoId,
                    ClientId = client.ClientId,  // Use the client ID retrieved from the accountId
                    ProviderId = requestDto.ProviderId,
                    VehicleId = vehicle.VehicleId,  // Set the VehicleId from the found vehicle
                    Status = requestDto.Status
                };

                await _context.ServiceRequests.AddAsync(serviceRequest);
                await _context.SaveChangesAsync();

                // Create the response DTO with additional client and vehicle info
                var serviceRequestResponse = new ServiceRequestDtoResponse
                {
                    ServiceRequestId = serviceRequest.ServiceRequestId,
                    ServiceInfoId = serviceRequest.ServiceInfoId,
                    ClientId = client.ClientId,
                    ClientName = client.Name,  
                    ProviderId = serviceRequest.ProviderId,
                    VehicleId = vehicle.VehicleId,
                    Status = serviceRequest.Status
                };

                return serviceRequestResponse;  // Return the ServiceRequestDtoResponse
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("An error occurred while creating the service request.", ex);
            }
        }


        public async Task DeleteServiceRequestAsync(int requestId)
        {
            try
            {
                var serviceRequest = await _context.ServiceRequests.FindAsync(requestId);
                if (serviceRequest != null)
                {
                    _context.ServiceRequests.Remove(serviceRequest);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception("An error occurred while deleting the service request.", ex);
            }
        }

        // Feedback management
        public async Task<IEnumerable<Feedback>> GetAllFeedbacksAsync()
        {
            return await _context.Feedbacks.ToListAsync();
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByServiceInfoIdAsync(int serviceInfoId)
        {
            return await _context.Feedbacks
                .Where(f => f.ServiceInfoId == serviceInfoId)
                .ToListAsync();
        }


        public async Task<IEnumerable<Feedback>> GetClientFeedbacksAsync(int clientId)
        {
            return await _context.Feedbacks
                .Where(f => f.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<Feedback> AddFeedbackAsync(FeedbackDto feedbackDto)
        {
            // Fetch the accountId from the current user's claims
            var accountId = GetAccountId();

            // Fetch the client based on the accountId
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.AccountId == accountId);

            // Check if the client exists
            if (client == null)
            {
                // Handle the case where the client does not exist
                throw new Exception("Client not found");
            }

            // Create the Feedback object and assign the ClientId
            var feedback = new Feedback
            {
                ClientId = client.ClientId,  // Use the AccountId as ClientId
                ServiceInfoId = feedbackDto.ServiceInfoId,
                Rating = feedbackDto.Rating,
                Comments = feedbackDto.Comments,
                Date = feedbackDto.Date
            };

            // Add and save feedback
            await _context.Feedbacks.AddAsync(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }


        public async Task RemoveFeedbackAsync(int feedbackId)
        {
            var feedback = await _context.Feedbacks.FindAsync(feedbackId);
            if (feedback != null)
            {
                _context.Feedbacks.Remove(feedback);
                await _context.SaveChangesAsync();
            }
        }

      

        // Notification management
        public async Task<IEnumerable<Notification>> GetClientNotificationsAsync()
        {
            var accountId = GetAccountId();
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.AccountId == accountId);

            if (client == null)
            {
                throw new UnauthorizedAccessException("Client not found");
            }

            return await _context.Notifications
                .Where(n => n.ClientId == client.ClientId)
                .ToListAsync();
        }

        public async Task<Notification> GetNotificationByIdAsync(int notificationId)
        {
            var accountId = GetAccountId();
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.AccountId == accountId);

            if (client == null)
            {
                throw new UnauthorizedAccessException("Client not found");
            }

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.NotificationId == notificationId && n.ClientId == client.ClientId);

            if (notification == null)
            {
                throw new KeyNotFoundException("Notification not found or does not belong to the client");
            }

            return notification;
        }
        public async Task DeleteNotificationAsync(int notificationId)
        {
            var accountId = GetAccountId();
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.AccountId == accountId);

            if (client == null)
            {
                throw new UnauthorizedAccessException("Client not found");
            }

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.NotificationId == notificationId && n.ClientId == client.ClientId);

            if (notification == null)
            {
                throw new KeyNotFoundException("Notification not found or does not belong to the client");
            }

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
        }


        //post managment 

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _context.Posts
                .Include(p => p.Account)          // Include Account to fetch UserName for the Post
                .Include(p => p.Comments)         // Include Comments collection
                    .ThenInclude(c => c.Account)
                .ToListAsync();
        }


        public async Task<PostResponseDto> AddPostAsync(PostDto postDto)
        {
            // Create a new Post instance from the provided PostDto
            var post = new Post
            {
                AccountId = postDto.AccountId, // Set AccountId instead of ClientId
                Title = postDto.Title,
                Content = postDto.Content,
                Date = postDto.Date
            };

            // Add the post to the context and save changes
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            // Fetch the user information to populate AccountId and UserName in PostResponseDto
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == post.AccountId); // Fetch the user by Id

            // Return the PostResponseDto
            return new PostResponseDto
            {
                PostId = post.PostId,
                AccountId = post.AccountId, // Use AccountId
                UserName = user != null ? user.UserName : "Unknown", // Use UserName from IdentityUser
                Title = post.Title,
                Content = post.Content,
                Date = post.Date,
                Comments = new List<CommentResponseDto>() // Initialize empty list
            };
        }


        public async Task<PostResponseDto> UpdatePostByIdAsync(int postId, PostDto postDto)
        {
            var post = await _context.Posts
                .Include(p => p.Comments) // Include Comments to avoid null references
                .FirstOrDefaultAsync(p => p.PostId == postId);

            if (post == null)
            {
                return null; // Handle post not found
            }

            // Update post fields
            post.Title = postDto.Title;
            post.Content = postDto.Content;
            post.Date = postDto.Date;

            await _context.SaveChangesAsync();

            // Fetch updated UserName from Account
            var userName = await _context.Users
                .Where(u => u.Id == post.AccountId)
                .Select(u => u.UserName)
                .FirstOrDefaultAsync();

            return new PostResponseDto
            {
                PostId = post.PostId,
                AccountId = post.AccountId,
                UserName = userName ?? "Unknown",
                Title = post.Title,
                Content = post.Content,
                Date = post.Date,
                Comments = post.Comments?.Select(comment => new CommentResponseDto
                {
                    CommentId = comment.CommentId,
                    AccountId = comment.AccountId,
                    PostId = comment.PostId,
                    Content = comment.Content,
                    Date = comment.Date,
                    UserName = comment.Account != null ? comment.Account.UserName : "Unknown"
                }).ToList() ?? new List<CommentResponseDto>() // Handle null Comments collection
            };
        }


        public async Task DeletePostAsync(int postId)
        {
            var post = await _context.Posts
                .Include(p => p.Comments) // Include comments to handle related data
                .FirstOrDefaultAsync(p => p.PostId == postId);

            if (post != null)
            {
                _context.Comments.RemoveRange(post.Comments); // Remove related comments
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CommentResponseDto>> GetAllCommentsAsync()
        {
            return await _context.Comments
                .Select(c => new CommentResponseDto
                {
                    CommentId = c.CommentId,
                    AccountId = c.AccountId,
                    PostId = c.PostId,
                    Content = c.Content,
                    Date = c.Date,
                    UserName = _context.Users
                        .Where(u => u.Id == c.AccountId)
                        .Select(u => u.UserName)
                        .FirstOrDefault()
                })
                .ToListAsync();
        }


        public async Task<CommentResponseDto> AddCommentAsync(CommentDto commentDto)
        {
            // Create and save the comment
            var comment = new Comment
            {
                AccountId = commentDto.AccountId,
                PostId = commentDto.PostId,
                Content = commentDto.Content,
                Date = commentDto.Date
            };

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            // Fetch the UserName from the AccountId
            var userName = await _context.Users
                .Where(u => u.Id == comment.AccountId)
                .Select(u => u.UserName) // Fetch UserName directly
                .FirstOrDefaultAsync();

            // Return the CommentResponseDto with UserName
            return new CommentResponseDto
            {
                CommentId = comment.CommentId,
                AccountId = comment.AccountId,
                PostId = comment.PostId,
                Content = comment.Content,
                Date = comment.Date,
                UserName = userName ?? "Unknown" // Use UserName or "Unknown" if null
            };
        }


        public async Task<CommentResponseDto> UpdateCommentByIdAsync(int commentId, CommentDto commentDto)
        {
            var comment = await _context.Comments.FindAsync(commentId);

            if (comment == null)
            {
                return null; // Handle comment not found
            }

            // Update comment fields
            comment.Content = commentDto.Content;
            comment.Date = commentDto.Date;

            await _context.SaveChangesAsync();

            // Fetch updated UserName from Account
            var userName = await _context.Users
                .Where(u => u.Id == comment.AccountId)
                .Select(u => u.UserName)
                .FirstOrDefaultAsync();

            return new CommentResponseDto
            {
                CommentId = comment.CommentId,
                AccountId = comment.AccountId,
                PostId = comment.PostId,
                Content = comment.Content,
                Date = comment.Date,
                UserName = userName ?? "Unknown"
            };
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }
    }
}