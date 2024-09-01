namespace MidProject.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; } // Foreign key
        public User User { get; set; } //Navigator
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
    }
}
