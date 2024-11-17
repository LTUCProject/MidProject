namespace MidProject.Models.Dto.Request
{
    public class ClientSubscriptionDto
    {
        public int SubscriptionPlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }
}
