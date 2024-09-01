namespace MidProject.Models.Dto.Request
{
    public class ServiceRequestDto
    {
        public int ServiceRequestId { get; set; }
        public int ServiceInfoId { get; set; } // Foreign key
        public int UserId { get; set; } // Foreign key
        public string Status { get; set; }
    }
}
