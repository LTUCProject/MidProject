namespace MidProject.Models.Dto.Response
{
    public class ClientSubscriptionResponseDto
    {
        public int ClientSubscriptionId { get; set; }
        public int ClientId { get; set; }
        public string SubscriptionPlanName { get; set; }
        public string SubscriptionPlanDescription { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string ClientName { get; internal set; }
    }
}
