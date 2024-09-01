namespace MidProject.Models
{
    public class SubscriptionPlan
    {
        public int SubscriptionPlanId { get; set; }
        public string PlanName { get; set; }
        public string Description { get; set; }
        public int MonthlyFee { get; set; }
        public string Benefits { get; set; }
        public ICollection<UserSubscription> UserSubscriptions { get; set; }
    }
}
