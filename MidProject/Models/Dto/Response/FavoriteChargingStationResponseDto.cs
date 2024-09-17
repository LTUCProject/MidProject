namespace MidProject.Models.Dto.Response
{
    public class FavoriteChargingStationResponseDto
    {
        public int FavoriteId { get; set; }
        public int ChargingStationId { get; set; }
        public int ClientId { get; set; }
        public string ChargingStationName { get; set; } // Only necessary properties
        public string StationLocation { get; set; }
        public bool HasParking { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
    }
}
