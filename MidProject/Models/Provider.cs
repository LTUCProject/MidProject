namespace MidProject.Models
{
    public class Provider
    {
        public int ProviderId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        //public string AccountId { get; set; } // Update to string
        public Account Account { get; set; } // Navigation property
        public ICollection<ServiceInfo> Services { get; set; }
        public ICollection<ServiceRequest> ServiceRequests { get; set; }
    }
}
