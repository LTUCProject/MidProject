using MidProject.Models;
using MidProject.Models.Dto.Request;

namespace MidProject.Repositories.Interfaces
{
    public interface IUserSubscription
    {
        Task<IEnumerable<UserSubscription>> GetAllUserSubscriptions();
        Task<UserSubscription> GetUserSubscriptionById(int id);
        Task AddUserSubscription(UserSubscriptionDto userSubscriptionDto);
        Task UpdateUserSubscription(UserSubscriptionDto userSubscriptionDto);
        Task DeleteUserSubscription(int id);
    }
}