namespace MidProject.Models.Dto.Request
{
    public class FeedbackDto
    {
        public int FeedbackId { get; set; }
        public int UserId { get; set; } // Foreign key
        public int ServiceInfoId { get; set; } // Foreign key
        public int Rating { get; set; }
        public string Comments { get; set; }
        public DateTime Date { get; set; }
    }
}
