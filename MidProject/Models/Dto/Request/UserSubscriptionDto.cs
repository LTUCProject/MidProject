namespace MidProject.Models.Dto.Request
{
    public class UserSubscriptionDto
    {
        public int UserSubscriptionId { get; set; }
        public int UserId { get; set; } // Foreign key
        public int SubscriptionPlanId { get; set; } // Foreign key
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }
}
