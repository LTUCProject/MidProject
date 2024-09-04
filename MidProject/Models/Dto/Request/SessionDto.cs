namespace MidProject.Models.Dto.Request
{
    public class SessionDto
    {
        public int SessionId { get; set; }
        public int ClientId { get; set; } // Foreign key
        public int ChargingStationId { get; set; } // Foreign key
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int EnergyConsumed { get; set; }
      
    }
}
