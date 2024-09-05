namespace MidProject.Models.Dto.Request2
{
    public class ChargingStationDto
    {
        public string StationLocation { get; set; }
        public int LocationId { get; set; }
        public string Name { get; set; }
        public bool HasParking { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
    }
}
