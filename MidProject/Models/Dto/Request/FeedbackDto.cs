namespace MidProject.Models.Dto.Request2
{
    public class FeedbackDto
    {
        public int ClientId { get; set; } // Foreign key
        public int ServiceInfoId { get; set; } // Foreign key
        public int Rating { get; set; }
        public string Comments { get; set; }
        public DateTime Date { get; set; }
    }
}
