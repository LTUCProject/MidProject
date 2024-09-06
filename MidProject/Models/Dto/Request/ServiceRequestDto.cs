using MidProject.Models.Dto.Response;
using System.ComponentModel.DataAnnotations;

namespace MidProject.Models.Dto.Request
{
    public class ServiceRequestDto
    {
        public int ServiceRequestId { get; set; }

        public int ServiceInfoId { get; set; }
        public ServiceInfoResponseDto ServiceInfo { get; set; }

        public int ClientId { get; set; }
        public ClientDto Client { get; set; } // Simplified Client DTO

        public ProviderDto Provider { get; set; } // Simplified Provider DTO

        public string Status { get; set; }
        public int ProviderId { get; internal set; }
    }
}
