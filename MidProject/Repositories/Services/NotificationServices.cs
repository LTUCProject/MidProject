using MidProject.Data;
using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class NotificationServices : INotification
    {
        private readonly MidprojectDbContext _context;

        public NotificationServices(MidprojectDbContext context) 
        {
            _context = context;
        }

        public Task AddNotification(NotificationDto notificationDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteNotification(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Notification>> GetAllNotifications()
        {
            throw new NotImplementedException();
        }

        public Task<Notification> GetNotificationById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateNotification(NotificationDto notificationDto)
        {
            throw new NotImplementedException();
        }
    }
}
