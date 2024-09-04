using System.ComponentModel.DataAnnotations;
namespace MidProject.Models
{
    public class ClientSubscription
    {
        public int ClientSubscriptionId { get; set; }

        [Required]
        public int ClientId { get; set; } // Changed to string to match IdentityUser key type
        public Client Client { get; set; }

        [Required]
        public int SubscriptionPlanId { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }
}