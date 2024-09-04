using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;
using MidProject.Models;
using MidProject.Repository.Interfaces;
using MidProject.Data;
using Microsoft.EntityFrameworkCore;

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
}
