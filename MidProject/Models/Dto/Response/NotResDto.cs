namespace MidProject.Models.Dto.Response
{
    public class NotResDto
    {
        public int NotificationId { get; set; }
        public int ClientId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }
}
