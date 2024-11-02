namespace MidProject.Models.Dto.Response
{
    public class ChargingStationResponseAdminDto
    {
        public int ChargingStationId { get; set; }
        public string StationLocation { get; set; }
        public string Address { get; set; } // Added Address
        public double Latitude { get; set; } // Added Latitude
        public double Longitude { get; set; } // Added Longitude
        public string Name { get; set; }
        public bool HasParking { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public List<ChargerResponseDto> Chargers { get; set; }
        public ProviderResponseDto Provider { get; set; }
    }
}
