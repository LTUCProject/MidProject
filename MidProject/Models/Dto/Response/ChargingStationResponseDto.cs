namespace MidProject.Models.Dto.Response
{
    public class ChargingStationResponseDto
    {
        public int ChargingStationId { get; set; }
        public string StationLocation { get; set; }
        public string Name { get; set; }
        public bool HasParking { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }

        // Location properties
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public ICollection<ChargerResponseDto> Chargers { get; set; } // Changed to ICollection<ChargerResponseDto>
    }
}
