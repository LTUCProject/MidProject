namespace MidProject.Models
{
    public class Charger
    {
        public int ChargerId { get; set; }
        public string Type { get; set; }
        public int Power { get; set; }
        public string Speed { get; set; }
        public int ChargingStationId { get; set; }
        public ChargingStation ChargingStation { get; set; }
    }
}
