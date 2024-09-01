using MidProject.Data;
using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class ServiceRequestServices : IServiceRequest
    {
        private readonly MidprojectDbContext _context;

        public ServiceRequestServices(MidprojectDbContext context) 
        {
            _context = context;
        }

        public Task AddServiceRequest(ServiceRequestDto serviceRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteServiceRequest(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ServiceRequest>> GetAllServiceRequests()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceRequest> GetServiceRequestById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateServiceRequest(ServiceRequestDto serviceRequestDto)
        {
            throw new NotImplementedException();
        }
    }
}
