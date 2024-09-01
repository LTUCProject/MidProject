using MidProject.Models;
using MidProject.Models.Dto.Request;

namespace MidProject.Repositories.Interfaces
{
    public interface ISubscriptionPlan
    {
        Task<IEnumerable<SubscriptionPlan>> GetAllSubscriptionPlans();
        Task<SubscriptionPlan> GetSubscriptionPlanById(int id);
        Task AddSubscriptionPlan(SubscriptionPlanDto subscriptionPlanDto);
        Task UpdateSubscriptionPlan(SubscriptionPlanDto subscriptionPlanDto);
        Task DeleteSubscriptionPlan(int id);
    }
}