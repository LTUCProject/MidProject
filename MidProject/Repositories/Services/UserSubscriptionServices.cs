using MidProject.Data;
using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class UserSubscriptionServices : IUserSubscription
    {
        private readonly MidprojectDbContext _context;

        public UserSubscriptionServices(MidprojectDbContext context) 
        {
            _context = context;
        }

        public Task AddUserSubscription(UserSubscriptionDto userSubscriptionDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserSubscription(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserSubscription>> GetAllUserSubscriptions()
        {
            throw new NotImplementedException();
        }

        public Task<UserSubscription> GetUserSubscriptionById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserSubscription(UserSubscriptionDto userSubscriptionDto)
        {
            throw new NotImplementedException();
        }
    }
}
