namespace MidProject.Models.Dto.Request2
{
    public class VehicleDto
    {
   //     public int VehicleId { get; set; }
        public string LicensePlate { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int BatteryCapacity { get; set; }
        public string ElectricType { get; set; }
        public int ClientId { get; set; }
        public int ServiceInfoId { get; set; } // Added
    }
}
