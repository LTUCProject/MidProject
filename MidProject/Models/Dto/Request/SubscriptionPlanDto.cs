using System.ComponentModel.DataAnnotations;

namespace MidProject.Models.Dto.Request
{
    public class SubscriptionPlanDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }
    }
}
