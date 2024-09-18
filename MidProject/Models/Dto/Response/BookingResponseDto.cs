using MidProject.Models;
using System;

namespace MidProject.Models.Dto.Response
{
    public class BookingResponseDto
    {
        public int BookingId { get; set; }
        public int ClientId { get; set; }
        public int ChargingStationId { get; set; } // Updated field
        public int VehicleId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public int Cost { get; set; }
    }
}
