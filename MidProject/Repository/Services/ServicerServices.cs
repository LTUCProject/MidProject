using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;
using MidProject.Models;
using MidProject.Repository.Interfaces;
using MidProject.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MidProject.Models.Dto.Request;

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

    public async Task<ServiceInfoDtoResponse> GetServiceByIdAsync(int serviceId)
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

    public async Task CreateServiceAsync(ServiceInfoDtoRequest serviceDto)
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

    public async Task UpdateServiceAsync(ServiceInfoDtoRequest serviceDto)
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

    public async Task DeleteServiceAsync(int serviceId)
    {
        var service = await _context.ServiceInfos.FindAsync(serviceId);
        if (service == null) return;

        _context.ServiceInfos.Remove(service);
        await _context.SaveChangesAsync();
    }

    // Service requests management
    public async Task<IEnumerable<ServiceRequestDtoResponse>> GetServiceRequestsAsync(int serviceId)
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

    public async Task AddServiceRequestAsync(ServiceRequestDtoRequest requestDto)
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

    public async Task RemoveServiceRequestAsync(int requestId)
    {
        var request = await _context.ServiceRequests.FindAsync(requestId);
        if (request == null) return;

        _context.ServiceRequests.Remove(request);
        await _context.SaveChangesAsync();
    }

    // Booking management
    public async Task<IEnumerable<BookingResponseDto>> GetBookingsAsync(int serviceId)
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

    public async Task<BookingResponseDto> GetBookingByIdAsync(int bookingId)
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

    public async Task AddBookingAsync(BookingDto bookingDto)
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

    public async Task RemoveBookingAsync(int bookingId)
    {
        var booking = await _context.Bookings.FindAsync(bookingId);
        if (booking == null) return;

        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();
    }


    // Vehicle management
    public async Task<IEnumerable<VehicleDtoResponse>> GetVehiclesAsync(int serviceId)
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

    public async Task<VehicleDtoResponse> GetVehicleByIdAsync(int vehicleId)
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

    public async Task AddVehicleAsync(VehicleDto vehicleDto)
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

    public async Task RemoveVehicleAsync(int vehicleId)
    {
        var vehicle = await _context.Vehicles.FindAsync(vehicleId);
        if (vehicle == null) return;

        _context.Vehicles.Remove(vehicle);
        await _context.SaveChangesAsync();
    }

    //Feedback
    public async Task<IEnumerable<FeedbackDtoResponse>> GetFeedbacksAsync(int serviceId)
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
}

