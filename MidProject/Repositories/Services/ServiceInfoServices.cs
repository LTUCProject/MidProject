using Microsoft.EntityFrameworkCore;
using MidProject.Data;
using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class ServiceInfoServices : IServiceInfo
    {
        private readonly MidprojectDbContext _context;

        public ServiceInfoServices(MidprojectDbContext context) 
        {
            _context = context;
        }

        public Task AddServiceInfo(ServiceInfoDto serviceInfoDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteServiceInfo(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ServiceInfo>> GetAllServiceInfos()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceInfo> GetServiceInfoById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateServiceInfo(ServiceInfoDto serviceInfoDto)
        {
            throw new NotImplementedException();
        }
    }

}
