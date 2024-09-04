namespace MidProject.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public int ClientId { get; set; } // Changed to string to match IdentityUser key type
        public Client Client { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }
}
