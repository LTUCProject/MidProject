namespace MidProject.Models.Dto.Request
{
    public class ClientServiceRequestDto
    {
        public int ServiceInfoId { get; set; } // Foreign key
        public int ClientId { get; set; } // Foreign key
        public int ProviderId { get; set; }

        public string Status { get; set; }

        // Added VehicleId
        public int VehicleId { get; set; } // Foreign key
    }
}
