namespace MidProject.Models.Dto.Response
{
    public class ChargingStationResponseAdminDto
    {
        public int ChargingStationId { get; set; }
        public string StationLocation { get; set; }
        public int LocationId { get; set; }
        public string Name { get; set; }
        public bool HasParking { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public List<ChargerResponseDto> Chargers { get; set; }
        public ProviderResponseDto Provider { get; set; }
    }
}
