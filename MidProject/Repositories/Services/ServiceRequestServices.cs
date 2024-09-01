using MidProject.Data;
using MidProject.Models;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class ServiceRequestServices : Repository<ServiceRequest>, IServiceRequest
    {
        private readonly MidprojectDbContext _context;

        public ServiceRequestServices(MidprojectDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
