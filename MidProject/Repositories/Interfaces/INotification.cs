using MidProject.Models;
using MidProject.Models.Dto.Request;

namespace MidProject.Repositories.Interfaces
{
    public interface INotification
    {
        Task<IEnumerable<Notification>> GetAllNotifications();
        Task<Notification> GetNotificationById(int id);
        Task AddNotification(NotificationDto notificationDto);
        Task UpdateNotification(NotificationDto notificationDto);
        Task DeleteNotification(int id);
    }
}