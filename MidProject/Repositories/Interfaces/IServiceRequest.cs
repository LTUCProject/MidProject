using MidProject.Models;
using MidProject.Models.Dto.Request;

namespace MidProject.Repositories.Interfaces
{
    public interface IServiceRequest
    {
        Task<IEnumerable<ServiceRequest>> GetAllServiceRequests();
        Task<ServiceRequest> GetServiceRequestById(int id);
        Task AddServiceRequest(ServiceRequestDto serviceRequestDto);
        Task UpdateServiceRequest(ServiceRequestDto serviceRequestDto);
        Task DeleteServiceRequest(int id);
    }
}