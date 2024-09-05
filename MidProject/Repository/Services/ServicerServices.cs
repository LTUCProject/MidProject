using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;
using MidProject.Models;
using MidProject.Repository.Interfaces;
using MidProject.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

public class ServicerService : IServicer
{
    private readonly MidprojectDbContext _context;

    public ServicerService(MidprojectDbContext context)
    {
        _context = context;
    }

    // Service management
    public async Task<IEnumerable<ServiceInfoDtoResponse>> GetAllServicesAsync()
    {
        try
        {
            var services = await _context.ServiceInfos.ToListAsync();
            return services.Select(s => new ServiceInfoDtoResponse
            {
                ServiceInfoId = s.ServiceInfoId,
                Name = s.Name,
                Description = s.Description,
                Contact = s.Contact,
                Type = s.Type,
                ProviderId = s.ProviderId
            });
        }
        catch (Exception ex)
        {
            // Log the exception (use a logging framework or service here)
            Console.WriteLine($"Error fetching services: {ex.Message}");
            return Enumerable.Empty<ServiceInfoDtoResponse>();
        }
    }

    public async Task<ServiceInfoDtoResponse> GetServiceByIdAsync(int serviceId)
    {
        try
        {
            var service = await _context.ServiceInfos.FindAsync(serviceId);
            if (service == null) return null;

            return new ServiceInfoDtoResponse
            {
                ServiceInfoId = service.ServiceInfoId,
                Name = service.Name,
                Description = service.Description,
                Contact = service.Contact,
                Type = service.Type,
                ProviderId = service.ProviderId
            };
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error fetching service by ID: {ex.Message}");
            return null;
        }
    }

    public async Task CreateServiceAsync(ServiceInfoDto serviceDto)
    {
        try
        {
            var service = new ServiceInfo
            {
                Name = serviceDto.Name,
                Description = serviceDto.Description,
                Contact = serviceDto.Contact,
                Type = serviceDto.Type,
                ProviderId = serviceDto.ProviderId
            };

            _context.ServiceInfos.Add(service);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error creating service: {ex.Message}");
            // Handle or rethrow as necessary
        }
    }

    public async Task UpdateServiceAsync(ServiceInfoDto serviceDto)
    {
        try
        {
            var service = await _context.ServiceInfos.FindAsync(serviceDto.ServiceInfoId);
            if (service == null) return;

            service.Name = serviceDto.Name;
            service.Description = serviceDto.Description;
            service.Contact = serviceDto.Contact;
            service.Type = serviceDto.Type;
            service.ProviderId = serviceDto.ProviderId;

            _context.ServiceInfos.Update(service);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error updating service: {ex.Message}");
            // Handle or rethrow as necessary
        }
    }

    public async Task DeleteServiceAsync(int serviceId)
    {
        try
        {
            var service = await _context.ServiceInfos.FindAsync(serviceId);
            if (service == null) return;

            _context.ServiceInfos.Remove(service);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error deleting service: {ex.Message}");
            // Handle or rethrow as necessary
        }
    }

    // Service requests management
    public async Task<IEnumerable<ServiceRequestDtoResponse>> GetServiceRequestsAsync(int serviceId)
    {
        try
        {
            var requests = await _context.ServiceRequests
                .Where(sr => sr.ServiceInfoId == serviceId)
                .ToListAsync();

            return requests.Select(r => new ServiceRequestDtoResponse
            {
                ServiceRequestId = r.ServiceRequestId,
                ServiceInfoId = r.ServiceInfoId,
                ClientId = r.ClientId,
                ProviderId = r.ProviderId,
                Status = r.Status
            });
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error fetching service requests: {ex.Message}");
            return Enumerable.Empty<ServiceRequestDtoResponse>();
        }
    }

    public async Task AddServiceRequestAsync(ServiceRequestDto requestDto)
    {
        try
        {
            var request = new ServiceRequest
            {
                ServiceInfoId = requestDto.ServiceInfoId,
                ClientId = requestDto.ClientId,
                ProviderId = requestDto.ProviderId,
                Status = requestDto.Status
            };

            _context.ServiceRequests.Add(request);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error adding service request: {ex.Message}");
            // Handle or rethrow as necessary
        }
    }

    public async Task RemoveServiceRequestAsync(int requestId)
    {
        try
        {
            var request = await _context.ServiceRequests.FindAsync(requestId);
            if (request == null) return;

            _context.ServiceRequests.Remove(request);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error removing service request: {ex.Message}");
            // Handle or rethrow as necessary
        }
    }

    // Booking management
    public async Task<IEnumerable<BookingResponseDto>> GetBookingsAsync(int serviceId)
    {
        try
        {
            var bookings = await _context.Bookings
                .Where(b => b.ServiceInfoId == serviceId)
                .ToListAsync();

            return bookings.Select(b => new BookingResponseDto
            {
                BookingId = b.BookingId,
                ClientId = b.ClientId,
                ServiceInfoId = b.ServiceInfoId,
                VehicleId = b.VehicleId,
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                Status = b.Status,
                Cost = b.Cost
            });
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error fetching bookings: {ex.Message}");
            return Enumerable.Empty<BookingResponseDto>();
        }
    }

    public async Task<BookingResponseDto> GetBookingByIdAsync(int bookingId)
    {
        try
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null) return null;

            return new BookingResponseDto
            {
                BookingId = booking.BookingId,
                ClientId = booking.ClientId,
                ServiceInfoId = booking.ServiceInfoId,
                VehicleId = booking.VehicleId,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                Status = booking.Status,
                Cost = booking.Cost
            };
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error fetching booking by ID: {ex.Message}");
            return null;
        }
    }

    public async Task AddBookingAsync(BookingDto bookingDto)
    {
        try
        {
            var booking = new Booking
            {
                ClientId = bookingDto.ClientId,
                ServiceInfoId = bookingDto.ServiceInfoId,
                VehicleId = bookingDto.VehicleId,
                StartTime = bookingDto.StartTime,
                EndTime = bookingDto.EndTime,
                Status = bookingDto.Status,
                Cost = bookingDto.Cost
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error adding booking: {ex.Message}");
            // Handle or rethrow as necessary
        }
    }

    public async Task RemoveBookingAsync(int bookingId)
    {
        try
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null) return;

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error removing booking: {ex.Message}");
            // Handle or rethrow as necessary
        }
    }

    // Vehicle management
    public async Task<IEnumerable<VehicleDtoResponse>> GetVehiclesAsync(int serviceId)
    {
        try
        {
            var vehicles = await _context.Vehicles
                .Where(v => v.ServiceInfoId == serviceId)
                .ToListAsync();

            return vehicles.Select(v => new VehicleDtoResponse
            {
                VehicleId = v.VehicleId,
                LicensePlate = v.LicensePlate,
                Model = v.Model,
                Year = v.Year,
                BatteryCapacity = v.BatteryCapacity,
                ElectricType = v.ElectricType,
                ClientId = v.ClientId,
                ServiceInfoId = v.ServiceInfoId
            });
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error fetching vehicles: {ex.Message}");
            return Enumerable.Empty<VehicleDtoResponse>();
        }
    }

    public async Task<VehicleDtoResponse> GetVehicleByIdAsync(int vehicleId)
    {
        try
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle == null) return null;

            return new VehicleDtoResponse
            {
                VehicleId = vehicle.VehicleId,
                LicensePlate = vehicle.LicensePlate,
                Model = vehicle.Model,
                Year = vehicle.Year,
                BatteryCapacity = vehicle.BatteryCapacity,
                ElectricType = vehicle.ElectricType,
                ClientId = vehicle.ClientId,
                ServiceInfoId = vehicle.ServiceInfoId
            };
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error fetching vehicle by ID: {ex.Message}");
            return null;
        }
    }

    public async Task AddVehicleAsync(VehicleDto vehicleDto)
    {
        try
        {
            var vehicle = new Vehicle
            {
                LicensePlate = vehicleDto.LicensePlate,
                Model = vehicleDto.Model,
                Year = vehicleDto.Year,
                BatteryCapacity = vehicleDto.BatteryCapacity,
                ElectricType = vehicleDto.ElectricType,
                ClientId = vehicleDto.ClientId,
                ServiceInfoId = vehicleDto.ServiceInfoId
            };

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error adding vehicle: {ex.Message}");
            // Handle or rethrow as necessary
        }
    }

    public async Task RemoveVehicleAsync(int vehicleId)
    {
        try
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle == null) return;

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error removing vehicle: {ex.Message}");
            // Handle or rethrow as necessary
        }
    }

    // Feedback management
    public async Task<IEnumerable<FeedbackDtoResponse>> GetFeedbacksAsync(int serviceId)
    {
        try
        {
            var feedbacks = await _context.Feedbacks
                .Where(f => f.ServiceInfoId == serviceId)
                .ToListAsync();

            return feedbacks.Select(f => new FeedbackDtoResponse
            {
                FeedbackId = f.FeedbackId,
                ClientId = f.ClientId,
                ServiceInfoId = f.ServiceInfoId,
                Rating = f.Rating,
                Comments = f.Comments,
                Date = f.Date
            });
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error fetching feedbacks: {ex.Message}");
            return Enumerable.Empty<FeedbackDtoResponse>();
        }
    }
    public async Task AddServiceInfoAsync(ServiceInfoDto serviceInfoDto)
    {
        var serviceInfo = new ServiceInfo
        {
            Name = serviceInfoDto.Name,
            Description = serviceInfoDto.Description,
            Contact = serviceInfoDto.Contact,
            Type = serviceInfoDto.Type,
            ProviderId = serviceInfoDto.ProviderId
        };

        _context.ServiceInfos.Add(serviceInfo);
        await _context.SaveChangesAsync();
    }
}
