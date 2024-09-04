namespace MidProject.Models.Dto.Response
{
    public class VehicleResponseDto
    {
        public int VehicleId { get; set; }
        public string LicensePlate { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int BatteryCapacity { get; set; }
        public string ElectricType { get; set; }
        public int ClientId { get; set; }
    }
}
