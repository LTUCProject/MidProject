using MidProject.Models.Dto.Request;
using MidProject.Models.Dto.Request2;

namespace MidProject.Models.Dto.Response
{
    public class BookingResponseDto2
    {
        public int BookingId { get; set; }

        public int ClientId { get; set; }
        public ClientDto Client { get; set; } // Simplified Client DTO

        public int ServiceInfoId { get; set; }
        public ServiceInfoResponseDto ServiceInfo { get; set; }

        public int VehicleId { get; set; }
        public VehicleDto Vehicle { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string Status { get; set; }
        public int Cost { get; set; }
    }
}
