namespace MidProject.Models
{
    public class UserSubscription
    {
        public int UserSubscriptionId { get; set; }
        public int UserId { get; set; } // Foreign key
        public User User { get; set; } //Navigator
        public int SubscriptionPlanId { get; set; } // Foreign key
        public SubscriptionPlan SubscriptionPlan { get; set; } //Navigator
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }
}