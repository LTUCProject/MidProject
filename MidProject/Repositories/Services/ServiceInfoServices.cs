using Microsoft.EntityFrameworkCore;
using MidProject.Data;
using MidProject.Models;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class ServiceInfoServices : Repository<ServiceInfo>, IServiceInfo
    {
        private readonly MidprojectDbContext _context;

        public ServiceInfoServices(MidprojectDbContext context) : base(context)
        {
            _context = context;
        }

    }

}
