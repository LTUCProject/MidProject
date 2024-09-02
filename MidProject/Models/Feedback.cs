namespace MidProject.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public int ClientId { get; set; } // Changed to string to match IdentityUser key type
        public Client Client { get; set; }
        public int ServiceInfoId { get; set; }
        public ServiceInfo ServiceInfo { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
        public DateTime Date { get; set; }
    }
}
