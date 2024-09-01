namespace MidProject.Models.Dto.Request
{
    public class SubscriptionPlanDto
    {
        public int SubscriptionPlanId { get; set; }
        public string PlanName { get; set; }
        public string Description { get; set; }
        public int MonthlyFee { get; set; }
        public string Benefits { get; set; }
    }
}
