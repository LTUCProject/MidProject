using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;
using System.ComponentModel.DataAnnotations;

namespace MidProject.Models.Dto.Request
{
    public class ServiceRequestDto
    {
        public int ServiceRequestId { get; set; }

        public int ServiceInfoId { get; set; } // Keep this field
        public ServiceInfoResponseDto ServiceInfo { get; set; }

        public int ClientId { get; set; }
        public ClientDto Client { get; set; } // Simplified Client DTO

        public ProviderDto Provider { get; set; } // Simplified Provider DTO

        [Required]
        public int VehicleId { get; set; } // Added VehicleId for the vehicle info

        public string Status { get; set; }
        public int ProviderId { get; set; }
        public VehicleDto Vehicle { get; internal set; }
    }
}
