namespace MidProject.Models.Dto.Response
{
    public class LocationResponseDto
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public IEnumerable<ChargingStationResponseDto> ChargingStations { get; set; }
    }
}
