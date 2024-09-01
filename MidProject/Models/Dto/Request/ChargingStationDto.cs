namespace MidProject.Models.Dto.Request
{
    public class ChargingStationDto
    {
        public int ChargingStationId { get; set; }
        public string StationLocation { get; set; }
        public int LocationId { get; set; }
        public string Name { get; set; }
        public bool HasParking { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }

    }
}
