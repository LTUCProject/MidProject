namespace MidProject.Models.Dto.Response
{
    public class BookingResponseDto
    {
        public int BookingId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }  // Assuming you want to return some client info
        public int ServiceInfoId { get; set; }
        public string ServiceName { get; set; }  // Assuming you want to return some service info
        public int VehicleId { get; set; }
        public string VehicleModel { get; set; }  // Assuming you want to return some vehicle info
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public int Cost { get; set; }
    }
}
