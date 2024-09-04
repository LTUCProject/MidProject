namespace MidProject.Models.Dto.Response
{
    public class FeedbackDtoResponse
    {
        public int FeedbackId { get; set; }
        public int ClientId { get; set; }
        public int ServiceInfoId { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
        public DateTime Date { get; set; }
    }
}
