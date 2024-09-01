using MidProject.Data;
using MidProject.Models;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class SessionServices : Repository<Session>, ISessionx
    {
        private readonly MidprojectDbContext _context;

        public SessionServices(MidprojectDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
