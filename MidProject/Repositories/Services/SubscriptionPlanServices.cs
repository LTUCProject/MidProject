using MidProject.Data;
using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class SubscriptionPlanServices : ISubscriptionPlan
    {
        private readonly MidprojectDbContext _context;

        public SubscriptionPlanServices(MidprojectDbContext context) 
        {
            _context = context;
        }

        public Task AddSubscriptionPlan(SubscriptionPlanDto subscriptionPlanDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteSubscriptionPlan(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SubscriptionPlan>> GetAllSubscriptionPlans()
        {
            throw new NotImplementedException();
        }

        public Task<SubscriptionPlan> GetSubscriptionPlanById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateSubscriptionPlan(SubscriptionPlanDto subscriptionPlanDto)
        {
            throw new NotImplementedException();
        }
    }
}
