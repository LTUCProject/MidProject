namespace MidProject.Models.Dto.Request2
{
    public class ChargingStationDto
    {
        public string StationLocation { get; set; } //
        public string Name { get; set; }//
        public bool HasParking { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }

        // Location properties
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
