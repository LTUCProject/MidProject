namespace MidProject.Models.Dto.Request2
{
    public class ServiceRequestDto
    {
     //   public int ServiceRequestId { get; set; }
        public int ServiceInfoId { get; set; } // Foreign key
        public int ClientId { get; set; } // Foreign key
        public int ProviderId { get; set; }

        public string Status { get; set; }
    }
}
