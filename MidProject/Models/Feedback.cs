namespace MidProject.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public int UserId { get; set; } // Foreign key
        public User User { get; set; } //Navigator
        public int ServiceInfoId { get; set; } // Foreign key
        public ServiceInfo ServiceInfo { get; set; } //Navigator
        public int Rating { get; set; }
        public string Comments { get; set; }
        public DateTime Date { get; set; }
    }
}
