using System.ComponentModel.DataAnnotations;

namespace MidProject.Models
{
    public class SubscriptionPlan
    {
        public int SubscriptionPlanId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }

        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue)]
        public int DurationInDays { get; set; }
        public ICollection<ClientSubscription> ClientSubscriptions { get; set; }
    }
}
