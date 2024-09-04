namespace MidProject.Models.Dto.Request
{
    public class ChargerDtoRequest
    {
        public string Type { get; set; }
        public int Power { get; set; }
        public string Speed { get; set; }
        public int ChargingStationId { get; set; }
    }
}
