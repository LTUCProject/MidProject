namespace MidProject.Models
{
    public class ClientSubscription
    {
        public int ClientSubscriptionId { get; set; }
        public int ClientId { get; set; } // Changed to string to match IdentityUser key type
        public Client Client { get; set; }
        public int SubscriptionPlanId { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }
}