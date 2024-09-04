namespace MidProject.Models.Dto.Response
{
    public class SubscriptionPlanResponseDto
    {
        public int SubscriptionPlanId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }
    }
}
