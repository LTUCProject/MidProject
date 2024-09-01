namespace MidProject.Models.Dto.Request
{
    public class NotificationDto
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; } // Foreign key
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
    }
}
