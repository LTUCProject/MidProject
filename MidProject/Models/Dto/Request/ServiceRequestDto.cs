using MidProject.Models;

namespace MidProject.Models.Dto.Request
{
    public class ServiceRequestDto
    {
        public int ServiceRequestId { get; set; }
        public int ServiceInfoId { get; set; } // Foreign key
        public int ClientId { get; set; } // Foreign key
        public int ProviderId { get; set; }

        public string Status { get; set; }
    }
}
