using MidProject.Models;
using MidProject.Models.Dto.Request;

namespace MidProject.Repositories.Interfaces
{
    public interface IServiceInfo
    {
        Task<IEnumerable<ServiceInfo>> GetAllServiceInfos();
        Task<ServiceInfo> GetServiceInfoById(int id);
        Task AddServiceInfo(ServiceInfoDto serviceInfoDto);
        Task UpdateServiceInfo(ServiceInfoDto serviceInfoDto);
        Task DeleteServiceInfo(int id);
    }
}