namespace MidProject.Models
{
    public class ServiceRequest
    {
        public int ServiceRequestId { get; set; }
        public int ServiceInfoId { get; set; } // Foreign key
        public ServiceInfo ServiceInfo { get; set; } //Navigator
        public int UserId { get; set; } // Foreign key
        public User User { get; set; } //Navigator
        public string Status { get; set; }
    }
}
