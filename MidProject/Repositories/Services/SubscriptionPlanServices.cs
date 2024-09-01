using MidProject.Data;
using MidProject.Models;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class SubscriptionPlanServices : Repository<SubscriptionPlan>, ISubscriptionPlan
    {
        private readonly MidprojectDbContext _context;

        public SubscriptionPlanServices(MidprojectDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
