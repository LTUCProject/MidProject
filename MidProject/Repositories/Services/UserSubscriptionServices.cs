using MidProject.Data;
using MidProject.Models;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class UserSubscriptionServices : Repository<UserSubscription>, IUserSubscription
    {
        private readonly MidprojectDbContext _context;

        public UserSubscriptionServices(MidprojectDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
