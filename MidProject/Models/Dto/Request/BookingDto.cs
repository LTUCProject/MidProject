﻿namespace MidProject.Models.Dto.Request
{
    public class BookingDto
    {
        public int BookingId { get; set; }
        public int UserId { get; set; } // Foreign key
        public int ServiceInfoId { get; set; } // Foreign key
        public int VehicleId { get; set; } // Foreign key
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public int Cost { get; set; }
    }
}
