namespace MidProject.Models.Dto.Response
{
    public class ChargerDtoResponse
    {
        public int ChargerId { get; set; }
        public string Type { get; set; }
        public int Power { get; set; }
        public string Speed { get; set; }
        public int ChargingStationId { get; set; }
    }
}
