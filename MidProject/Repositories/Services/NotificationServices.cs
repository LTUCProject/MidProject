using MidProject.Data;
using MidProject.Models;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class NotificationServices : Repository<Notification>, INotification
    {
        private readonly MidprojectDbContext _context;

        public NotificationServices(MidprojectDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
