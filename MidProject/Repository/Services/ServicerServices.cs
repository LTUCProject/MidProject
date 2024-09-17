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
using MidProject.Models.Dto.Request;

public class ServicerService : IServicer
{
    private readonly MidprojectDbContext _context;

    public ServicerService(MidprojectDbContext context)
    {
        _context = context;
    }

    // Service management

    public async Task<ServiceInfoResponseDto> CreateServiceInfoAsync(ServiceInfoRequestDto serviceInfoDto)
    {
        var serviceInfo = new ServiceInfo
        {
            Name = serviceInfoDto.Name,
            Description = serviceInfoDto.Description,
            Contact = serviceInfoDto.Contact,
            Type = serviceInfoDto.Type,
            ProviderId = serviceInfoDto.ProviderId,
        };

        _context.ServiceInfos.Add(serviceInfo);
        await _context.SaveChangesAsync();

        return new ServiceInfoResponseDto
        {
            ServiceInfoId = serviceInfo.ServiceInfoId,
            Name = serviceInfo.Name,
            Description = serviceInfo.Description,
            Contact = serviceInfo.Contact,
            Type = serviceInfo.Type,

        };
    }

    public async Task<ServiceInfoResponseDto> GetServiceInfoByIdAsync(int serviceInfoId)
    {
        var serviceInfo = await _context.ServiceInfos
            .Include(s => s.Provider)
            .FirstOrDefaultAsync(s => s.ServiceInfoId == serviceInfoId);

        if (serviceInfo == null)
        {
            return null;
        }

        return new ServiceInfoResponseDto
        {
            ServiceInfoId = serviceInfo.ServiceInfoId,
            Name = serviceInfo.Name,
            Description = serviceInfo.Description,
            Contact = serviceInfo.Contact,
            Type = serviceInfo.Type,

        };
    }

    public async Task<IEnumerable<ServiceInfoResponseDto>> GetAllServiceInfosAsync()
    {
        return await _context.ServiceInfos
            .Include(s => s.Provider)
            .Select(serviceInfo => new ServiceInfoResponseDto
            {
                ServiceInfoId = serviceInfo.ServiceInfoId,
                Name = serviceInfo.Name,
                Description = serviceInfo.Description,
                Contact = serviceInfo.Contact,
                Type = serviceInfo.Type,



            }).ToListAsync();
    }

    public async Task<bool> UpdateServiceInfoAsync(int serviceInfoId, ServiceInfoRequestDto serviceInfoDto)
    {
        var serviceInfo = await _context.ServiceInfos.FindAsync(serviceInfoId);

        if (serviceInfo == null)
        {
            return false;
        }

        serviceInfo.Name = serviceInfoDto.Name;
        serviceInfo.Description = serviceInfoDto.Description;
        serviceInfo.Contact = serviceInfoDto.Contact;
        serviceInfo.Type = serviceInfoDto.Type;
        serviceInfo.ProviderId = serviceInfoDto.ProviderId;

        _context.ServiceInfos.Update(serviceInfo);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteServiceInfoAsync(int serviceInfoId)
    {
        var serviceInfo = await _context.ServiceInfos.FindAsync(serviceInfoId);

        if (serviceInfo == null)
        {
            return false;
        }

        _context.ServiceInfos.Remove(serviceInfo);
        await _context.SaveChangesAsync();

        return true;
    }

    // Service requests management

    public async Task<ServiceRequestDto> CreateServiceRequestAsync(ServiceRequestDto serviceRequestDto)
    {
        var serviceRequest = new ServiceRequest
        {
            ServiceInfoId = serviceRequestDto.ServiceInfoId,
            ClientId = serviceRequestDto.ClientId,
            ProviderId = serviceRequestDto.ProviderId,
            Status = serviceRequestDto.Status
        };

        _context.ServiceRequests.Add(serviceRequest);
        await _context.SaveChangesAsync();

        return serviceRequestDto;
    }

    public async Task<ServiceRequestDto> GetServiceRequestByIdAsync(int serviceRequestId)
    {
        var serviceRequest = await _context.ServiceRequests
            .Include(sr => sr.ServiceInfo)
            .Include(sr => sr.Client)
            .Include(sr => sr.Provider)
            .FirstOrDefaultAsync(sr => sr.ServiceRequestId == serviceRequestId);

        if (serviceRequest == null)
        {
            return null;
        }

        return new ServiceRequestDto
        {
            ServiceRequestId = serviceRequest.ServiceRequestId,
            ServiceInfoId = serviceRequest.ServiceInfoId,
            ClientId = serviceRequest.ClientId,
            ProviderId = serviceRequest.ProviderId,
            Status = serviceRequest.Status,
            ServiceInfo = new ServiceInfoResponseDto
            {
                ServiceInfoId = serviceRequest.ServiceInfo.ServiceInfoId,
                Name = serviceRequest.ServiceInfo.Name,
                Description = serviceRequest.ServiceInfo.Description,
                Contact = serviceRequest.ServiceInfo.Contact,
                Type = serviceRequest.ServiceInfo.Type
            },
            Client = new ClientDto
            {
                ClientId = serviceRequest.Client.ClientId,
                Name = serviceRequest.Client.Name,
                Email = serviceRequest.Client.Email,

            }
        };
    }

    public async Task<IEnumerable<ServiceRequestDto>> GetServiceRequestsByServiceInfoIdAsync(int serviceInfoId)
    {
        return await _context.ServiceRequests
            .Where(sr => sr.ServiceInfoId == serviceInfoId)
            .Include(sr => sr.ServiceInfo)
            .Include(sr => sr.Client)
            .Include(sr => sr.Provider)
            .Include(sr => sr.Vehicle) // Ensure Vehicle is included
            .Select(serviceRequest => new ServiceRequestDto
            {
                ServiceRequestId = serviceRequest.ServiceRequestId,
                ServiceInfoId = serviceRequest.ServiceInfoId,
                ClientId = serviceRequest.ClientId,
                ProviderId = serviceRequest.ProviderId,
                Status = serviceRequest.Status,

                // Mapping ServiceInfo
                ServiceInfo = new ServiceInfoResponseDto
                {
                    ServiceInfoId = serviceRequest.ServiceInfo.ServiceInfoId,
                    Name = serviceRequest.ServiceInfo.Name,
                    Description = serviceRequest.ServiceInfo.Description,
                    Contact = serviceRequest.ServiceInfo.Contact,
                    Type = serviceRequest.ServiceInfo.Type
                },

                // Mapping Client
                Client = new ClientDto
                {
                    ClientId = serviceRequest.Client.ClientId,
                    Name = serviceRequest.Client.Name,
                    Email = serviceRequest.Client.Email,
                },

                // Mapping Provider
                Provider = new ProviderDto
                {
                    ProviderId = serviceRequest.Provider.ProviderId,
                    Name = serviceRequest.Provider.Name,
                },

                // Mapping Vehicle
                Vehicle = new VehicleDto
                {
                    VehicleId = serviceRequest.Vehicle.VehicleId,
                    LicensePlate = serviceRequest.Vehicle.LicensePlate,
                    Model = serviceRequest.Vehicle.Model,
                    Year = serviceRequest.Vehicle.Year,
                    BatteryCapacity = serviceRequest.Vehicle.BatteryCapacity,
                    ElectricType = serviceRequest.Vehicle.ElectricType
                }
            })
            .ToListAsync();
    }



    public async Task<bool> UpdateServiceRequestStatusAsync(int serviceRequestId, string status)
    {
        var serviceRequest = await _context.ServiceRequests.FindAsync(serviceRequestId);

        if (serviceRequest == null)
        {
            return false;
        }

        serviceRequest.Status = status;
        _context.ServiceRequests.Update(serviceRequest);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteServiceRequestAsync(int serviceRequestId)
    {
        var serviceRequest = await _context.ServiceRequests.FindAsync(serviceRequestId);

        if (serviceRequest == null)
        {
            return false;
        }

        _context.ServiceRequests.Remove(serviceRequest);
        await _context.SaveChangesAsync();

        return true;
    }

    // Booking management

    public async Task<BookingResponseDto2> CreateBookingAsync(BookingRequestDto bookingDto)
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

        return new BookingResponseDto2
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

    public async Task<BookingResponseDto2> GetBookingByIdAsync(int bookingId)
    {
        var booking = await _context.Bookings
            .Include(b => b.Client)
            .Include(b => b.ServiceInfo)
            .Include(b => b.Vehicle)
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);

        if (booking == null)
        {
            return null;
        }

        return new BookingResponseDto2
        {
            BookingId = booking.BookingId,
            ClientId = booking.ClientId,
            ServiceInfoId = booking.ServiceInfoId,
            VehicleId = booking.VehicleId,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            Status = booking.Status,
            Cost = booking.Cost,
            Client = new ClientDto
            {
                ClientId = booking.Client.ClientId,
                Name = booking.Client.Name,
                Email = booking.Client.Email,
            },
            ServiceInfo = new ServiceInfoResponseDto
            {
                ServiceInfoId = booking.ServiceInfo.ServiceInfoId,
                Name = booking.ServiceInfo.Name,
                Description = booking.ServiceInfo.Description,
                Contact = booking.ServiceInfo.Contact,
                Type = booking.ServiceInfo.Type
            },
            Vehicle = new VehicleDto
            {
                VehicleId = booking.Vehicle.VehicleId,
                Model = booking.Vehicle.Model,
                LicensePlate = booking.Vehicle.LicensePlate
            }
        };
    }

    public async Task<IEnumerable<BookingResponseDto2>> GetBookingsByClientIdAsync(int clientId)
    {
        return await _context.Bookings
            .Where(b => b.ClientId == clientId)
            .Include(b => b.Client)
            .Include(b => b.ServiceInfo)
            .Include(b => b.Vehicle)
            .Select(booking => new BookingResponseDto2
            {
                BookingId = booking.BookingId,
                ClientId = booking.ClientId,
                ServiceInfoId = booking.ServiceInfoId,
                VehicleId = booking.VehicleId,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                Status = booking.Status,
                Cost = booking.Cost,
                Client = new ClientDto
                {
                    ClientId = booking.Client.ClientId,
                    Name = booking.Client.Name,
                    Email = booking.Client.Email,
                },
                ServiceInfo = new ServiceInfoResponseDto
                {
                    ServiceInfoId = booking.ServiceInfo.ServiceInfoId,
                    Name = booking.ServiceInfo.Name,
                    Description = booking.ServiceInfo.Description,
                    Contact = booking.ServiceInfo.Contact,
                    Type = booking.ServiceInfo.Type
                },
                Vehicle = new VehicleDto
                {
                    VehicleId = booking.Vehicle.VehicleId,
                    Model = booking.Vehicle.Model,
                    LicensePlate = booking.Vehicle.LicensePlate
                }
            }).ToListAsync();
    }

    public async Task<IEnumerable<BookingResponseDto2>> GetBookingsByServiceInfoIdAsync(int serviceInfoId)
    {
        return await _context.Bookings
            .Where(b => b.ServiceInfoId == serviceInfoId)
            .Include(b => b.Client)
            .Include(b => b.ServiceInfo)
            .Include(b => b.Vehicle)
            .Select(booking => new BookingResponseDto2
            {
                BookingId = booking.BookingId,
                ClientId = booking.ClientId,
                ServiceInfoId = booking.ServiceInfoId,
                VehicleId = booking.VehicleId,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                Status = booking.Status,
                Cost = booking.Cost,
                Client = new ClientDto
                {
                    ClientId = booking.Client.ClientId,
                    Name = booking.Client.Name,
                    Email = booking.Client.Email,
                },
                ServiceInfo = new ServiceInfoResponseDto
                {
                    ServiceInfoId = booking.ServiceInfo.ServiceInfoId,
                    Name = booking.ServiceInfo.Name,
                    Description = booking.ServiceInfo.Description,
                    Contact = booking.ServiceInfo.Contact,
                    Type = booking.ServiceInfo.Type
                },
                Vehicle = new VehicleDto
                {
                    VehicleId = booking.Vehicle.VehicleId,
                    Model = booking.Vehicle.Model,
                    LicensePlate = booking.Vehicle.LicensePlate
                }
            }).ToListAsync();
    }

    public async Task<bool> UpdateBookingStatusAsync(int bookingId, string status)
    {
        var booking = await _context.Bookings.FindAsync(bookingId);

        if (booking == null)
        {
            return false;
        }

        booking.Status = status;
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CancelBookingAsync(int bookingId)
    {
        var booking = await _context.Bookings.FindAsync(bookingId);

        if (booking == null)
        {
            return false;
        }

        booking.Status = "Cancelled";
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();

        return true;
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



}



//// Feedback management
//public async Task<IEnumerable<FeedbackDtoResponse>> GetFeedbacksAsync(int serviceId)
//{
//    try
//    {
//        var feedbacks = await _context.Feedbacks
//            .Where(f => f.ServiceInfoId == serviceId)
//            .ToListAsync();

//        return feedbacks.Select(f => new FeedbackDtoResponse
//        {
//            FeedbackId = f.FeedbackId,
//            ClientId = f.ClientId,
//            ServiceInfoId = f.ServiceInfoId,
//            Rating = f.Rating,
//            Comments = f.Comments,
//            Date = f.Date
//        });
//    }
//    catch (Exception ex)
//    {
//        // Log the exception
//        Console.WriteLine($"Error fetching feedbacks: {ex.Message}");
//        return Enumerable.Empty<FeedbackDtoResponse>();
//    }
//}