﻿using System;
using System.ComponentModel.DataAnnotations;

namespace MidProject.Models.Dto.Response
{
    public class BookingDto
    {
        public int BookingId { get; set; }

        [Required]
        public int ClientId { get; set; }
        public string ClientName { get; internal set; }
        public string ClientEmail { get; internal set; }
        public string VehicleModel { get; internal set; }

        [Required]
        public int ChargingStationId { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Default to "Pending"

        [Range(0, int.MaxValue)]
        public int Cost { get; set; } = 0; // Default to 0

    }
}